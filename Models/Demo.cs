namespace portfolio_api.Models;

public class Demo
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Stakeholder { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Year { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public string DemoUrl { get; set; } = string.Empty;
    public List<string> StacksUsed { get; set; } = [];
    public string? GithubLink { get; set; } = string.Empty;
    public DateOnly CreatedDate { get; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? ModifiedDate { get; set; } = null;
}

public record PostDemoDTO(
    string Title,
    string? Stakeholder,
    string Category,
    string? Year,
    string? Description,
    string DemoUrl,
    List<string> StacksUsed,
    string? GithubLink
);