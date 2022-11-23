using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class DishDto
{
    public Guid id { get; set; }
    [Required]
    [MinLength(1)]
    public string name { get; set; }
    public string? description { get; set; }
    [Required]
    public double price { get; set; }
    public string? image { get; set; }
    [Required]
    public bool vegetarian { get; set; }
    public double? rating { get; set; }
    [Required]
    public DishCategory category { get; set; }
}