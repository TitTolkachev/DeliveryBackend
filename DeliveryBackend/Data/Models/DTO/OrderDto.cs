using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class OrderDto
{
    public Guid id { get; set; }
    [Required]
    public DateTime deliveryTime { get; set; }
    [Required]
    public DateTime orderTime { get; set; }
    [Required]
    public OrderStatus status { get; set; }
    [Required]
    public double price { get; set; }
    [Required]
    public List<DishBasketDto?> dishes { get; set; }
    [Required]
    [MinLength(1)]
    public string address { get; set; }
}