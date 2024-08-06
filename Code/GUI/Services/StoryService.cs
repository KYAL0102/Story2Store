using GUI.Entities;

namespace GUI.Services;

public class StoryService
{
    public Dictionary<int, string> GetAllStoryIDsAndNames()
    {
        return new Dictionary<int, string>
        {
            { 1, "A night sky" },
            { 2, "A last dance" },
            { 3, "No one knows" },
            { 4, "Christmas tale" }
        };
    }

    public async Task<Story> GetStoryWithId(int id)
    {
        return new Story
        {
            Title = "A night sky",
            Components = 
            [
                new TextField
                {
                    Content = "The sky today is"
                },
                new TextOption
                {
                    Options = 
                    [
                        "beautiful!",
                        "awesome!",
                        "blue!"
                    ]
                }
            ]
        };
    }
}