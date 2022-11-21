using DeliveryBackend.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBackend.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().HasKey(x => x.Id);
    }
}