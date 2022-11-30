using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.Data.Models.DTO.Queries;

public class GetDishListQuery
{
    public List<DishCategory> categories { get; set; } = new ();
    public bool vegetarian { get; set; } = false;
    public DishSorting? sorting { get; set; } = null;
    public int page { get; set; } = 1;
}