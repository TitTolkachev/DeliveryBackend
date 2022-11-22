namespace DeliveryBackend.Data.Models.DTO;

public class OrderCreateDto
{
    public DateTime deliveryTime { get; set; }
    public string address { get; set; }
}