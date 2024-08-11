using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using ClassLibrary;
using ClassLibrary.Entities;

namespace GUI.Services;

public class StoryService(HttpClient http)
{
    public async Task<List<StoryLight>?> GetAllStoryLightData()
    {
        var stories = await http.GetFromJsonAsync<List<StoryLight>>("api/Story/All");
        return stories;
    }

    public async Task<Story?> GetStoryWithId(int id)
    {
        var story = await http.GetFromJsonAsync<Story>($"api/Story/{id}");
        return story;
    }

    public async Task UpdateStory(Story story)
    {
        string json = JsonSerializer.Serialize(story, GlobalConstants.JsonOption);
        string username = "Joe";
        
        using StringContent content = new(
            json,
            Encoding.UTF8,
            "application/json"
        );

        var response = await http.PatchAsync($"api/Story/UpdateStory?username={Uri.EscapeDataString(username)}", content);
        
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task<int> AddStory(Story story)
    {
        int id = 0;
        string json = JsonSerializer.Serialize(story, GlobalConstants.JsonOption);
        string username = "Joe";

        using StringContent content = new(
            json,
            Encoding.UTF8,
            "application/json"
            );

        var response = await http.PostAsync($"api/Story/AddStory?username={Uri.EscapeDataString(username)}", content);

        try
        {
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            id = int.Parse(result);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
        catch (FormatException)
        {
            return -1;
        }

        return id;
    }

    public async Task<bool> DeleteStory(int id)
    {
        string username = "Joe";

        try
        {
            var response = await http.DeleteAsync($"api/Story/DeleteStory/{id}?username={Uri.EscapeDataString(username)}");
            
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}