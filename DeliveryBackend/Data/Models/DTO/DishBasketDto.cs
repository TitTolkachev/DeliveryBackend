namespace DeliveryBackend.Data.Models.DTO;

public class DishBasketDto
{
    public Guid id { get; set; }
    public string name { get; set; }
    public double price { get; set; }
    public double totalPrice { get; set; }
    public int amount { get; set; }
    public string? image { get; set; }
}