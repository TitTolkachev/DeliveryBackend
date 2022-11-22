using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class OrderInfoDto
{
    public Guid id { get; set; }
    public DateTime deliveryTime { get; set; }
    public DateTime orderTime { get; set; }
    public OrderStatus status { get; set; }
    public double price { get; set; }
}