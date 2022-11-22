using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class DishDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public string? description { get; set; }
    public double price { get; set; }
    public string? image { get; set; }
    public bool vegetarian { get; set; }
    public double? rating { get; set; }
    public DishCategory category { get; set; }
}