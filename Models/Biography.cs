namespace portfolio_api.Models;

public class Biography
{
    public string FirstName { get; set; } = string.Empty;
    public string? MiddleName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBitrh { get; set; }
    public string Domicile { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string[] Languages { get; set; } = new string[4];
    public string ProfessionalSummary { get; set; } = string.Empty;
    public List<Experience> Experiences { get; set; } = [];
    public List<Education> Educations { get; set; } = [];
    public List<Certification> Certifications { get; set; } = [];
    public List<Skill> Skills { get; set; } = [];
    public List<BioProject>? Projects { get; set; }
    public List<Award>? Awards { get; set; }
    public List<Publication>? Publications { get; set; }
    public Biography() { }

}

public record Experience(string YearRange, string Company, List<string> Responsibilities, string Role, List<string>? TechStacks);
public record Education(string YearRange, string Degree, string School, string? GPA);
public record Certification(string Year, string Name, string Issuer, string CredentialLink);
public record Skill(string Category, List<string> Skills);
public record BioProject(string Name, string Description, List<string> Contributions, string Role, List<string> TechStacks);
public record Award(string Name, string Year, string Issuer);
public record Publication(string Year, string Title, string Publisher);
