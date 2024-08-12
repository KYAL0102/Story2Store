using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SQLite;
using ClassLibrary.Entities;

namespace Core.Managers;

public class SqLiteManager(string connectionString)
{
    private SQLiteConnection Connection { get; init; } = new()
    {
        ConnectionString = connectionString
    };

    public async Task<IList<StoryLight>> GetAllStories()
    {
        var result = new List<StoryLight>();
        var wasOpen = Connection.State == ConnectionState.Open;
        if (!wasOpen) await Connection.OpenAsync();

        var sql = "SELECT * FROM StoryLightView";
        
        try
        {
            await using var command = new SQLiteCommand(sql, Connection);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var story = new StoryLight
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    ComponentCount = reader.GetInt32(2)
                };

                result.Add(story);
            }
            
        }
        catch (DbException e)
        {
            throw new SQLiteException(e.Message);
        }
        finally
        {
            if (!wasOpen) await Connection.CloseAsync();
        }

        return result.ToList();
    }

    public async Task<Story?> GetStoryById(int id)
    {
        Story? story = null;
        var wasOpen = Connection.State == ConnectionState.Open;
        if (!wasOpen) await Connection.OpenAsync();

        var sql = "SELECT Id, Title FROM Story WHERE Id = @id";

        try
        {
            await using var command1 = new SQLiteCommand(sql, Connection);
            command1.Parameters.AddWithValue("@id", id);

            await using var reader1 = await command1.ExecuteReaderAsync();
            while (await reader1.ReadAsync())
            {
                story = new Story
                {
                    Id = reader1.GetInt32(0),
                    Title = reader1.GetString(1),
                    Components = new()
                };
            }

            if (story == null) return story;

            sql = "SELECT Type, Guid, Content FROM TextComponent WHERE StoryId == @storyId";

            await using var command2 = new SQLiteCommand(sql, Connection);
            command2.Parameters.AddWithValue("@storyId", id);

            await using var reader2 = await command2.ExecuteReaderAsync();
            while (await reader2.ReadAsync())
            {
                TextComponent component;
                var type = Type.GetType(reader2.GetString(0));

                if (type == typeof(TextField))
                {
                    component = new TextField
                    {
                        Guid = Guid.Parse(reader2.GetString(1)),
                        Content = reader2.GetString(2)
                    };
                }
                else if (type == typeof(TextOption))
                {
                    component = new TextOption
                    {
                        Guid = Guid.Parse(reader2.GetString(1)),
                        Options = reader2.GetString(2)
                                            .Split(";")
                                            .ToList()
                    };
                }
                else
                {
                    return story;
                }

                story.Components.Add(component);
            }
        }
        catch (DbException e)
        {
            throw new SQLiteException(e.Message);
        }
        finally
        {
            if (!wasOpen) await Connection.CloseAsync();
        }

        return story;
    }

    public async Task<int> AddNewStory(string username, Story story)
    {
        var returnValue = -1;
        var wasOpen = Connection.State == ConnectionState.Open;
        if (!wasOpen) await Connection.OpenAsync();
        
        var sql = "INSERT INTO Story (Title, Username) VALUES (@title, @username) RETURNING ROWID";

        try
        {
            await using var command1 = new SQLiteCommand(sql, Connection);
            command1.Parameters.AddWithValue("@title", story.Title);
            command1.Parameters.AddWithValue("@username", username);

            var result = (long) (await command1.ExecuteScalarAsync() ?? -1);
            var storyId = (int) result;

            sql = "INSERT INTO TextComponent (Guid, Content, Type, StoryId) VALUES (@guid, @content, @type, @storyId)";

            await using var command2 = new SQLiteCommand(sql, Connection);
            command2.Parameters.Add(new SQLiteParameter("@guid", DbType.String));
            command2.Parameters.Add(new SQLiteParameter("@content", DbType.String));
            command2.Parameters.Add(new SQLiteParameter("@type", DbType.String));
            command2.Parameters.Add(new SQLiteParameter("@storyId", DbType.Int32));
            
            foreach (var component in story.Components)
            {
                command2.Parameters["@guid"].Value = component.Guid.ToString();

                if (component.GetType() == typeof(TextField))
                {
                    var textField = (TextField)component;
                    
                    command2.Parameters["@content"].Value = textField.Content;
                    command2.Parameters["@type"].Value = typeof(TextField).AssemblyQualifiedName;
                    command2.Parameters["@storyId"].Value = storyId;
                }
                else if (component.GetType() == typeof(TextOption))
                {
                    var option = (TextOption)component;
                    
                    command2.Parameters["@content"].Value = string.Join(";", option.Options);
                    command2.Parameters["@type"].Value = typeof(TextOption).AssemblyQualifiedName;
                    command2.Parameters["@storyId"].Value = storyId;
                }
                
                Console.WriteLine(command2.Parameters["@content"].Value);

                await command2.ExecuteNonQueryAsync();
            }

            returnValue = storyId;
        }
        catch (DbException e)
        {
            throw new SQLiteException(e.Message);
        }
        finally
        {
            if (!wasOpen) await Connection.CloseAsync();
        }

        return returnValue;
    }

    public async Task UpdateStory(Story story)
    {
        if (story.Id < 0) return;
        var wasOpen = Connection.State == ConnectionState.Open;
        if (!wasOpen) await Connection.OpenAsync();

        var storySql = "UPDATE Story SET Title = @title WHERE Id = @id";
        var removeOldComponentsSql = "DELETE FROM TextComponent WHERE StoryId = @storyId";
        var componentSql = "INSERT INTO TextComponent (Guid, Content, Type, StoryId) VALUES (@guid, @content, @type, @storyId)";

        try
        {
            await using var storyCommand = new SQLiteCommand(storySql, Connection);
            await using var removeComponentsCommand = new SQLiteCommand(removeOldComponentsSql, Connection);
            await using var componentCommand = new SQLiteCommand(componentSql, Connection);
            
            storyCommand.Parameters.AddWithValue("@id", story.Id);
            storyCommand.Parameters.AddWithValue("@title", story.Title);
            
            removeComponentsCommand.Parameters.AddWithValue("@storyId", story.Id);
            
            componentCommand.Parameters.AddWithValue("@storyId", story.Id);
            componentCommand.Parameters.Add(new SQLiteParameter("@guid", DbType.String));
            componentCommand.Parameters.Add(new SQLiteParameter("@content", DbType.String));
            componentCommand.Parameters.Add(new SQLiteParameter("@type", DbType.String));

            var task = storyCommand.ExecuteNonQueryAsync();
            await removeComponentsCommand.ExecuteNonQueryAsync();
            await task;

            foreach (var component in story.Components)
            {
                componentCommand.Parameters["@guid"].Value = component.Guid.ToString();

                if (component.GetType() == typeof(TextField))
                {
                    var textField = (TextField) component;
                    componentCommand.Parameters["@content"].Value = textField.Content;
                    componentCommand.Parameters["@type"].Value = typeof(TextField).AssemblyQualifiedName;
                }
                else if (component.GetType() == typeof(TextOption))
                {
                    var option = (TextOption) component;
                    componentCommand.Parameters["@content"].Value = string.Join(";", option.Options);
                    componentCommand.Parameters["@type"].Value = typeof(TextOption).AssemblyQualifiedName;
                }

                await componentCommand.ExecuteNonQueryAsync();
            }
        }
        catch (DbException e)
        {
            throw new SQLiteException(e.Message);
        }
        finally
        {
            if (!wasOpen) await Connection.CloseAsync();
        }
    }

    public async Task DeleteStory(int id)
    {
        var wasOpen = Connection.State == ConnectionState.Open;
        if (!wasOpen) await Connection.OpenAsync();

        var sql = "DELETE FROM TextComponent WHERE StoryId = @storyId";
        var sql2 = "DELETE FROM Story WHERE Id = @id";
        
        try
        {
            await using var componentCommand = new SQLiteCommand(sql, Connection);
            await using var storyCommand = new SQLiteCommand(sql2, Connection);
            componentCommand.Parameters.AddWithValue("@storyId", id);
            storyCommand.Parameters.AddWithValue("@id", id);

            var task = componentCommand.ExecuteNonQueryAsync();
            await storyCommand.ExecuteNonQueryAsync();
            await task;
        }
        catch (DbException e)
        {
            throw new SQLiteException(e.Message);
        }
        finally
        {
            if (!wasOpen) await Connection.CloseAsync();
        }
    }
}