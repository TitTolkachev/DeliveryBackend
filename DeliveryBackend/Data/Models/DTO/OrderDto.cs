using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO;

public class OrderDto
{
    public Guid id { get; set; }
    public DateTime deliveryTime { get; set; }
    public DateTime orderTime { get; set; }
    public OrderStatus status { get; set; }
    public double price { get; set; }
    public List<DishBasketDto?> dishes { get; set; }
    public string address { get; set; }
}