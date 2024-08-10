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

    public async Task AddStory(Story story)
    {
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
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}