using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using portfolio_api.Context;
using portfolio_api.Helpers;
using portfolio_api.Models;
using portfolio_api.ViewModels;

namespace portfolio_api.Controllers;

[Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly AppDbContext _context;
    private readonly ImageUpload _imageUpload;
    private readonly JsonFileReader<SiteSettings> _settingsReader;
    private readonly JsonFileWriter<SiteSettings> _settingsWriter;

    public HomeController(AppDbContext context, ImageUpload imageUpload, JsonFileReader<SiteSettings> settingsReader, JsonFileWriter<SiteSettings> settingsWriter)
    {
        _context = context;
        _imageUpload = imageUpload;
        _settingsReader = settingsReader;
        _settingsWriter = settingsWriter;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? editProject, int? editDemo)
    {
        var model = new HomeIndexViewModel
        {
            Projects = await _context.Projects.OrderByDescending(x => x.Id).ToListAsync(),
            Demos = await _context.Demos.OrderByDescending(x => x.Id).ToListAsync(),
            Project = default!,
            Demo = default!,
            EditProjectId = editProject,
            EditDemoId = editDemo
        };

        if (editProject is int pid)
        {
            var p = await _context.Projects.FindAsync(pid);
            if (p is not null)
            {
                model.Project = new FormProjectDTO(
                    p.Title, p.Description, p.Category,
                    p.DateInitiated, p.DatePublished,
                    p.StackUsed, null, null, p.Link);
                model.ExistingProjectImages = p.Images ?? [];
                model.ExistingProjectVideos = p.Videos ?? [];
            }
        }

        if (editDemo is int did)
        {
            var d = await _context.Demos.FindAsync(did);
            if (d is not null)
            {
                model.Demo = new PostDemoDTO(
                    d.Title, d.Stakeholder, d.Category,
                    d.Year, d.Description, d.DemoUrl,
                    d.StacksUsed, d.GithubLink);
            }
        }

        model.Project ??= new FormProjectDTO("", "", "", "", "", [], null, null, null);
        model.Demo ??= new PostDemoDTO("", null, "", null, null, "", [], null);

        model.SiteSettings = await _settingsReader.ReadAsync();

        ViewData["Title"] = "Admin";
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProject([FromForm] FormProjectDTO project)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        _context.Projects.Add(new Project
        {
            Title = project.Title,
            Description = project.Description,
            Category = project.Category,
            DateInitiated = project.DateInitiated,
            DatePublished = project.DatePublished,
            StackUsed = FilterList(project.StackUsed),
            Images = await SaveFilesAsync(project.Images),
            Videos = await SaveFilesAsync(project.Videos),
            Link = project.Link
        });
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProject(int id, [FromForm] FormProjectDTO project, [FromForm] List<string>? existingImages, [FromForm] List<string>? existingVideos)
    {
        var entity = await _context.Projects.FindAsync(id);
        if (entity is null) return RedirectToAction(nameof(Index));

        List<string> keptImages = FilterList(existingImages);
        List<string> keptVideos = FilterList(existingVideos);

        foreach (string path in FilterList(entity.Images).Except(keptImages)) _imageUpload.DeleteFile(path);
        foreach (string path in FilterList(entity.Videos).Except(keptVideos)) _imageUpload.DeleteFile(path);

        entity.Title = project.Title;
        entity.Description = project.Description;
        entity.Category = project.Category;
        entity.DateInitiated = project.DateInitiated;
        entity.DatePublished = project.DatePublished;
        entity.StackUsed = FilterList(project.StackUsed);
        entity.Images = keptImages.Concat(await SaveFilesAsync(project.Images)).ToList();
        entity.Videos = keptVideos.Concat(await SaveFilesAsync(project.Videos)).ToList();
        entity.Link = project.Link;
        entity.ModifiedDate = DateOnly.FromDateTime(DateTime.Now);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var entity = await _context.Projects.FindAsync(id);
        if (entity is not null)
        {
            foreach (string path in FilterList(entity.Images)) _imageUpload.DeleteFile(path);
            foreach (string path in FilterList(entity.Videos)) _imageUpload.DeleteFile(path);

            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateDemo([FromForm] PostDemoDTO demo)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }

        _context.Demos.Add(new Demo
        {
            Title = demo.Title,
            Stakeholder = demo.Stakeholder,
            Category = demo.Category,
            Year = demo.Year,
            Description = demo.Description,
            DemoUrl = demo.DemoUrl,
            StacksUsed = FilterList(demo.StacksUsed),
            GithubLink = demo.GithubLink
        });
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDemo(int id, [FromForm] PostDemoDTO demo)
    {
        var entity = await _context.Demos.FindAsync(id);
        if (entity is null) return RedirectToAction(nameof(Index));

        entity.Title = demo.Title;
        entity.Stakeholder = demo.Stakeholder;
        entity.Category = demo.Category;
        entity.Year = demo.Year;
        entity.Description = demo.Description;
        entity.DemoUrl = demo.DemoUrl;
        entity.StacksUsed = FilterList(demo.StacksUsed);
        entity.GithubLink = demo.GithubLink;
        entity.ModifiedDate = DateOnly.FromDateTime(DateTime.Now);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDemo(int id)
    {
        var entity = await _context.Demos.FindAsync(id);
        if (entity is not null)
        {
            _context.Demos.Remove(entity);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSiteSetting([FromForm] SiteSettings siteSettings)
    {
        siteSettings.TypedTexts = FilterList(siteSettings.TypedTexts);
        siteSettings.MadeWith = FilterList(siteSettings.MadeWith);
        siteSettings.SocialLinks = siteSettings.SocialLinks
            .Where(s => !string.IsNullOrWhiteSpace(s.Name) || !string.IsNullOrWhiteSpace(s.Label) || !string.IsNullOrWhiteSpace(s.Url))
            .Select(s => new SocialLink
            {
                Name = s.Name.Trim(),
                Label = s.Label.Trim(),
                Url = s.Url.Trim()
            })
            .ToList();
        siteSettings.GalleryItems = siteSettings.GalleryItems
            .Where(g => !string.IsNullOrWhiteSpace(g.Id) || !string.IsNullOrWhiteSpace(g.Title))
            .Select(g => new GalleryItem
            {
                Id = g.Id.Trim(),
                Title = g.Title.Trim(),
                Numbering = g.Numbering,
                Description = g.Description.Trim(),
                Image = g.Image?.Trim(),
                Video = g.Video?.Trim()
            })
            .ToList();

        await _settingsWriter.WriteAsync(siteSettings);
        return RedirectToAction(nameof(Index));
    }

    private static List<string> FilterList(List<string>? values)
    {
        if (values is null) return [];
        return values.Select(v => v.Trim()).Where(v => v.Length > 0).ToList();
    }

    private async Task<List<string>> SaveFilesAsync(List<IFormFile>? files)
    {
        var paths = new List<string>();
        if (files is null) return paths;

        foreach (var file in files)
        {
            string? path = await _imageUpload.SaveImageAsync(file);
            if (path is not null) paths.Add(path);
        }

        return paths;
    }
}