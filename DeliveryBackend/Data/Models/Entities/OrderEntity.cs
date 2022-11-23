using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }
    [Required]
    public DateTime DeliveryTime { get; set; }
    [Required]
    public DateTime OrderTime { get; set; }
    [Required]
    public OrderStatus Status { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public List<DishEntity?> Dishes { get; set; }
    [Required]
    [MinLength(1)]
    public string Address { get; set; }
    [Required]
    public UserEntity User { get; set; }
}