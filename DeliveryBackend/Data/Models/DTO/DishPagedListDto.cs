using System.ComponentModel.DataAnnotations;

namespace DeliveryBackend.Data.Models.DTO;

public class DishPagedListDto
{
    [Required]
    public List<DishDto?> dishes { get; set; }
    [Required]
    public PageInfoModel paginagion { get; set; }
}