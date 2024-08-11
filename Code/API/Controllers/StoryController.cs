using System.Data.SQLite;
using ClassLibrary.Entities;
using Core.Managers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoryController(SqLiteManager sqLiteManager) : ControllerBase
{
    [HttpGet("All")]
    public async Task<IActionResult> GetAllStories()
    {
        try
        {
            var stories = await sqLiteManager.GetAllStories();
            return Ok(stories);
        }
        catch (SQLiteException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStoryById([FromRoute] int id)
    {
        try
        {
            var story = await sqLiteManager.GetStoryById(id);
            return Ok(story);
        }
        catch (SQLiteException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("AddStory")]
    public async Task<IActionResult> AddStory([FromQuery] string username, [FromBody] Story story)
    {
        try
        {
            var id = await sqLiteManager.AddNewStory(username, story);
            return Ok(id);
        }
        catch (SQLiteException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPatch("UpdateStory")]
    public async Task<IActionResult> UpdateStory([FromQuery] string username, [FromBody] Story story)
    {
        try
        {
            await sqLiteManager.UpdateStory(story);
        }
        catch (SQLiteException e)
        {
            return BadRequest(e.Message);
        }
        return Ok("Story successfully updated!");
    }
    
    [HttpDelete("DeleteStory/{id}")]
    public async Task<IActionResult> DeleteStory([FromQuery] string username, [FromRoute] int id)
    {
        try
        {
            await sqLiteManager.DeleteStory(id);
        }
        catch (SQLiteException e)
        {
            return BadRequest(e.Message);
        }
        return Ok("Story deleted.");
    }
}