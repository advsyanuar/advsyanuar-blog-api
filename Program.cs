namespace portfolio_api;

public static class Program
{
    public static void Main(string[] args)
    {
        const string cors_policy = "advsyanuar-frontend";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: cors_policy, builder =>
            {
                builder.WithOrigins("http://localhost:5173", "http://advsyanuar.cloud");
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseCors(cors_policy);

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
