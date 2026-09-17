using portfolio_api.Models;

namespace portfolio_api.ViewModels;

public class HomeIndexViewModel
{
    public List<Project> Projects { get; set; } = [];
    public List<Demo> Demos { get; set; } = [];

    public FormProjectDTO Project { get; set; } = default!;
    public FormProjectDTO FormProject { get; set; } = default!;
    public PostDemoDTO Demo { get; set; } = default!;

    public SiteSettings SiteSettings { get; set; } = new();

    public List<string> ExistingProjectImages { get; set; } = [];
    public List<string> ExistingProjectVideos { get; set; } = [];

    public int? EditProjectId { get; set; }
    public int? EditDemoId { get; set; }
}