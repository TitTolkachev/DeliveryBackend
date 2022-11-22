namespace DeliveryBackend.Data.Models.DTO;

public class DishPagedListDto
{
    public List<DishDto?> dishes { get; set; }
    public PageInfoModel paginagion { get; set; }
}