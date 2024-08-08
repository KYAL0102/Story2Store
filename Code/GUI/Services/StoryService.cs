using GUI.Entities;

namespace GUI.Services;

public class StoryService
{
    public List<StoryLight> GetAllStoryLightData()
    {
        return
        [
            new StoryLight
            {
                Id = 1,
                Title = "A night sky",
                ComponentCount = 4
            },

            new StoryLight
            {
                Id = 2,
                Title = "A last dance",
                ComponentCount = 15
            },

            new StoryLight
            {
                Id = 3,
                Title = "No one knows",
                ComponentCount = 12
            },

            new StoryLight
            {
                Id = 4,
                Title = "Christmas tale",
                ComponentCount = 21
            }
        ];
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