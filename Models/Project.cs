namespace portfolio_api.Models;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string DateInitiated { get; set; } = string.Empty;
    public string DatePublished { get; set; } = string.Empty;
    public List<string>? Images { get; set; }
    public List<string>? Videos { get; set; }
    public string? Link { get; set; }
    public List<string> StackUsed { get; set; } = [];
    public DateOnly CreatedDate { get; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? ModifiedDate { get; set; } = null;
};

// DTOs
public record PostProjectDTO(string Title, string Description, string Category, string DateInitiated, string DatePublished, List<string> StackUsed, List<string>? Images, List<string>? Videos, string? Link);
public record FormProjectDTO(string Title, string Description, string Category, string DateInitiated, string DatePublished, List<string> StackUsed, List<IFormFile>? Images, List<IFormFile>? Videos, string? Link);
