using ClassLibrary.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoryController : ControllerBase
{
    [HttpGet("All")]
    public IActionResult GetAllStories()
    {
        var stories = new[] {
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
        };

        return Ok(stories);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetStoryById([FromRoute] int id)
    {
        var story = new Story
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
        return Ok(story);
    }
    
    [HttpPost("AddStory")]
    public IActionResult AddStory([FromQuery] string username, [FromBody] Story story)
    {
        return Ok(1);
    }
    
    [HttpPatch("UpdateStory")]
    public IActionResult UpdateStory([FromQuery] string username, [FromBody] Story story)
    {
        return Ok(1);
    }
    
    [HttpDelete("DeleteStory/{id}")]
    public IActionResult DeleteStory([FromQuery] string username, [FromRoute] Story story)
    {
        return Ok("Story deleted.");
    }
}