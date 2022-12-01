using DeliveryBackend.Data.Models.Enums;

namespace DeliveryBackend.DTO.Queries;

public class GetDishListQuery
{
    public List<DishCategory> Categories { get; set; } = new ();
    public bool Vegetarian { get; set; } = false;
    public DishSorting? Sorting { get; set; } = null;
    public int Page { get; set; } = 1;
}