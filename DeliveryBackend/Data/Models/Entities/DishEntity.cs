using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.Entities;

public class DishEntity
{
    public Guid Id { get; set; }
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public double Price { get; set; }
    public string Image { get; set; }
    [Required]
    public bool Vegetarian { get; set; }
    [Required]
    public DishCategory Category { get; set; }
    
    public List<CartEntity> Carts { get; set; }
    public List<RatingEntity> Ratings { get; set; }
}