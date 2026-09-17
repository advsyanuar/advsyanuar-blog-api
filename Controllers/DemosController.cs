using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Context;
using portfolio_api.Models;

namespace portfolio_api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DemosController : ControllerBase
{
    private readonly AppDbContext _context;
    private static int _nextId = 1;

    public DemosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int limit = 10)
    {
        List<Demo> demos = await _context.Demos.Take(limit).OrderByDescending(x => x.Id).ToListAsync<Demo>();
        return Ok(demos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PostDemoDTO value)
    {
        Demo demo = new()
        {
            Id = _nextId++,
            Title = value.Title,
            Stakeholder = value.Stakeholder,
            Category = value.Category,
            Year = value.Year,
            Description = value.Description,
            DemoUrl = value.DemoUrl,
            StacksUsed = value.StacksUsed,
            GithubLink = value.GithubLink
        };
        _context.Demos.Add(demo);
        await _context.SaveChangesAsync();
        return Created();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        Demo? demo = await _context.Demos.FindAsync(id);
        if (demo == null)
        {
            return NotFound();
        }
        _context.Demos.Remove(demo);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PostDemoDTO value)
    {
        Demo? demo = await _context.Demos.FindAsync(id);
        if (demo == null)
        {
            return NotFound();
        }
        demo.Title = value.Title;
        demo.Stakeholder = value.Stakeholder;
        demo.Category = value.Category;
        demo.Year = value.Year;
        demo.Description = value.Description;
        demo.DemoUrl = value.DemoUrl;
        demo.StacksUsed = value.StacksUsed;
        demo.GithubLink = value.GithubLink;
        demo.ModifiedDate = DateOnly.FromDateTime(DateTime.Now);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}
