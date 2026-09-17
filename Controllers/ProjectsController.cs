using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Context;
using portfolio_api.Models;

namespace portfolio_api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;
    private static int _nextId = 1;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 10)
    {
        List<Project> projects = await _context.Projects.Take(limit).OrderBy(x => x.Id).ToListAsync<Project>();
        return Ok(projects);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostProjectDTO value)
    {
        Project project = new()
        {
            Id = _nextId++,
            Title = value.Title,
            Description = value.Description,
            Category = value.Category,
            DateInitiated = value.DateInitiated,
            DatePublished = value.DatePublished,
            Images = value.Images,
            Videos = value.Videos,
            Link = value.Link,
            StackUsed = value.StackUsed,
            ModifiedDate = DateOnly.FromDateTime(DateTime.Now)
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return Created();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Patch([FromBody] PostProjectDTO value, int id)
    {
        Project project = await _context.Projects.FirstAsync(x => x.Id == id);
        var (Title, Description, Category, DateInitiated, DatePublished, StackUsed, Images, Videos, Link) = value;
        if (Title is not null) project.Title = Title;
        if (Description is not null) project.Description = Description;
        if (Category is not null) project.Category = Category;
        if (DateInitiated is not null) project.DateInitiated = DateInitiated;
        if (DatePublished is not null) project.DatePublished = DatePublished;
        if (Images is not null) project.Images = Images;
        if (Videos is not null) project.Videos = Videos;
        if (Link is not null) project.Link = Link;
        if (StackUsed is not null) project.StackUsed = StackUsed;
        project.ModifiedDate = DateOnly.FromDateTime(DateTime.Now);
        _context.Projects.Update(project);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Project project = await _context.Projects.FirstAsync(x => x.Id == id);
        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return Ok();
    }


}