using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeliveryBackend.Data.Models.Entities;

public class CartEntity
{
    public Guid Id { get; set; }
    
    [Required]
    public Guid DishId { get; set; }
    [Required]
    [ForeignKey("DishId")]
    public DishEntity DishEntity { get; set; }
    
    [Required]
    public int Amount { get; set; }
    
    [Required]
    public Guid UserId { get; set; }
    [Required]
    [ForeignKey("UserId")]
    public UserEntity UserEntity { get; set; }
    
    [Required]
    public Guid? OrderId { get; set; }
    [Required]
    [ForeignKey("OrderId")]
    public OrderEntity? OrderEntity { get; set; }
}