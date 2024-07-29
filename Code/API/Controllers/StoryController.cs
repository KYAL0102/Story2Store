using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoryController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetStoryById([FromRoute] int id)
    {
        var story = new Story();
        return Ok(story);
    }
    
    [HttpPost("AddStory")]
    public IActionResult AddStory([FromQuery] string username, [FromBody] string[] text)
    {
        var story = new Story
        {
            Text = text
        };
        return Ok(1);
    }
    
    [HttpPost("DeleteStory/{id}")]
    public IActionResult AddStory([FromQuery] string username, [FromRoute] int id)
    {
        return Ok("Story deleted.");
    }
}