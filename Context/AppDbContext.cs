using Microsoft.EntityFrameworkCore;
using portfolio_api.Models;

namespace portfolio_api.Context;

public class AppDbContext : DbContext
{
    public DbSet<Demo> Demos { get; set; }
    public DbSet<Project> Projects { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
