using System.ComponentModel.DataAnnotations;

namespace DeliveryBackend.Data.Models.DTO;

public class OrderCreateDto
{
    [Required]
    public DateTime deliveryTime { get; set; }
    [Required]
    [MinLength(1)]
    public string address { get; set; }
}