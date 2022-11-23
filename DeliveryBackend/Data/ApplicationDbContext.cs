using DeliveryBackend.Data.Models;
using DeliveryBackend.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBackend.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<RatingEntity> Ratings { get; set; }
    public DbSet<DishEntity> Dishes { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CartEntity> Carts { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>().HasKey(x => x.Id);

        modelBuilder.Entity<RatingEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<RatingEntity>()
            .HasIndex(x => new { x.DishId, x.UserId })
            .IsUnique();
        
        modelBuilder.Entity<DishEntity>().HasKey(x => x.Id);
        
        modelBuilder.Entity<OrderEntity>().HasKey(x => x.Id);
        
        modelBuilder.Entity<UserEntity>().HasKey(x => x.Id);
        
        modelBuilder.Entity<CartEntity>().HasKey(x => x.Id);
        modelBuilder.Entity<CartEntity>()
            .HasIndex(x => new { x.DishId, x.UserId, x.OrderId })
            .IsUnique();
    }
}