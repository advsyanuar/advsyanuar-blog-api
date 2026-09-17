namespace portfolio_api.Models;

public class SiteSettings
{
    public string SiteLogo { get; set; } = string.Empty;
    public List<string> TypedTexts { get; set; } = [];
    public List<SocialLink> SocialLinks { get; set; } = [];
    public List<GalleryItem> GalleryItems { get; set; } = [];
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string>? MadeWith { get; set; } = [];
    public bool BackgroundVideoEnabled { get; set; } = false;
    public string? BackgroundUrl { get; set; }
}

public class GalleryItem
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Numbering { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string? Video { get; set; }
}

public class SocialLink
{
    public string Name { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}