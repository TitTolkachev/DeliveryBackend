using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    public DateTime? BirthDate { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    [Required]
    [MinLength(1)]
    [EmailAddress]
    public string Email { get; set; }
    public string? Address { get; set; }
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    public List<RatingEntity> Ratings { get; set; }
    public List<OrderEntity> Orders { get; set; }
    public List<CartEntity> Carts { get; set; }
}