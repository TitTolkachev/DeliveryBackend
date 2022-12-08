﻿using DeliveryBackend.Data.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBackend.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Token> Tokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>().HasKey(x => x.Id);
        modelBuilder.Entity<Rating>()
            .HasIndex(x => new { x.DishId, x.UserId })
            .IsUnique();
        
        modelBuilder.Entity<Dish>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Order>().HasKey(x => x.Id);
        
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        
        modelBuilder.Entity<Cart>().HasKey(x => x.Id);
        modelBuilder.Entity<Cart>()
            .HasIndex(x => new { x.DishId, x.UserId, x.OrderId })
            .IsUnique();
        modelBuilder.Entity<Cart>()
            .Property(x=>x.OrderId)
            .IsRequired(false);

        modelBuilder.Entity<Token>().HasKey(x => x.InvalidToken);
    }
}