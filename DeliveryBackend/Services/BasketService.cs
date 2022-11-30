using DeliveryBackend.Data;
using DeliveryBackend.Data.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBackend.Services.Interfaces;

public class BasketService : IBasketService
{
    private readonly ApplicationDbContext _context;

    public BasketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DishBasketDto>> GetUserCart(Guid userId)
    {
        //TODO(Накидать Exceptions)
        var dishList = await _context.Carts.Where(x => x.UserId == userId).Join(
                _context.Dishes, 
                c => c.DishId, 
                d => d.Id,
                (c, d) => new DishBasketDto
                {
                    id = c.Id,
                    name = d.Name,
                    price = d.Price,
                    totalPrice = d.Price*c.Amount,
                    amount = c.Amount,
                    image = d.Image
                }
            )
            .ToListAsync();

        return dishList;
    }

    public async Task AddDishToCart(Guid dishId, Guid userId)
    {
    }

    public async Task RemoveDishFromCart(Guid dishId, Guid userId)
    {
    }
}