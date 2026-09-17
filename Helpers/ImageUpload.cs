namespace portfolio_api.Helpers;

public class ImageUpload
{
    private readonly IWebHostEnvironment _environment;

    public ImageUpload(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string?> SaveImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0) return null;

        string uploadFolder = Path.Combine(_environment.WebRootPath, "storage");
        if (!Directory.Exists(uploadFolder)) Directory.CreateDirectory(uploadFolder);

        string extension = Path.GetExtension(image.FileName);
        string uniqueFilename = Guid.NewGuid().ToString() + extension;
        string filepath = Path.Combine(uploadFolder, uniqueFilename);

        await using var filestream = new FileStream(filepath, FileMode.Create);
        await image.CopyToAsync(filestream);

        return "/storage/" + uniqueFilename;
    }

    public void DeleteFile(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;

        string filepath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/'));
        if (File.Exists(filepath)) File.Delete(filepath);
    }
}