using System.ComponentModel.DataAnnotations;
using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class OrderInfoDto
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
}