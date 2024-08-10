using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using ClassLibrary.Entities;

namespace Core.Managers;

public class SqLiteManager(string connectionString)
{
    private SQLiteConnection Connection { get; init; } = new()
    {
        ConnectionString = connectionString
    };

    public async Task<int> AddNewStory(string username, string title, List<TextField> textFields, List<TextOption> textOptions)
    {
        var returnValue = -1;
        var wasOpen = Connection.State == ConnectionState.Open;
        if (!wasOpen) await Connection.OpenAsync();
        
        var sql = "INSERT INTO Story (Title, Username) VALUES (@title, @username) RETURNING ROWID";

        try
        {
            await using var command1 = new SQLiteCommand(sql, Connection);
            command1.Parameters.AddWithValue("@title", title);
            command1.Parameters.AddWithValue("@username", username);

            var result = (long) (await command1.ExecuteScalarAsync() ?? -1);
            var storyId = (int) result;

            sql = "INSERT INTO TextComponent (Guid, Content, Type, StoryId) VALUES (@guid, @content, @type, @storyId)";

            await using var command2 = new SQLiteCommand(sql, Connection);
            command2.Parameters.Add(new SQLiteParameter("@guid", DbType.String));
            command2.Parameters.Add(new SQLiteParameter("@content", DbType.String));
            command2.Parameters.Add(new SQLiteParameter("@type", DbType.String));
            command2.Parameters.Add(new SQLiteParameter("@storyId", DbType.Int32));

            var combinedList = new List<TextComponent>();
            combinedList.AddRange(textFields);
            combinedList.AddRange(textOptions);
            
            foreach (var component in combinedList)
            {
                command2.Parameters["@guid"].Value = component.Guid.ToString();

                if (component.GetType() == typeof(TextField))
                {
                    var textField = (TextField)component;
                    
                    command2.Parameters["@content"].Value = textField.Content;
                    command2.Parameters["@type"].Value = nameof(TextField);
                    command2.Parameters["@storyId"].Value = storyId;
                }
                else if (component.GetType() == typeof(TextOption))
                {
                    var option = (TextOption)component;
                    
                    command2.Parameters["@content"].Value = string.Join(";", option.Options);
                    command2.Parameters["@type"].Value = nameof(TextOption);
                    command2.Parameters["@storyId"].Value = storyId;
                }
                
                Console.WriteLine(command2.Parameters["@content"].Value);

                await command2.ExecuteNonQueryAsync();
            }

            returnValue = storyId;
        }
        catch (DbException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            if (!wasOpen) await Connection.CloseAsync();
        }

        return returnValue;
    }
}