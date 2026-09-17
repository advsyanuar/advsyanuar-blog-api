using Microsoft.EntityFrameworkCore;
using portfolio_api.Context;
using portfolio_api.Helpers;

namespace portfolio_api;

public static class Program
{
    public static void Main(string[] args)
    {
        const string cors_policy = "advsyanuar-frontend";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        string[] corsOrigins = (builder.Configuration["Cors:AllowedOrigins"] ?? "http://localhost:5173,http://advsyanuar.cloud")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: cors_policy, builder =>
            {
                builder.WithOrigins(corsOrigins);
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<ImageUpload>();
        var app = builder.Build();

        // Apply pending EF Core migrations on startup (creates DB + tables if they don't exist)
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

        app.UseCors(cors_policy);

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
