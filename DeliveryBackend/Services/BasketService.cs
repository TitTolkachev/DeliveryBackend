using DeliveryBackend.Data;
using DeliveryBackend.Data.Models.DTO;
using DeliveryBackend.Data.Models.Entities;
using DeliveryBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBackend.Services;

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
                    totalPrice = d.Price * c.Amount,
                    amount = c.Amount,
                    image = d.Image
                }
            )
            .ToListAsync();

        return dishList;
    }

    public async Task AddDishToCart(Guid dishId, Guid userId)
    {
        //TODO(Накидать Exceptions)
        var dishCartEntity =
            await _context.Carts.Where(x => x.UserId == userId && x.DishId == dishId).FirstOrDefaultAsync();

        if (dishCartEntity == null)
        {
            await _context.Carts.AddAsync(new CartEntity
            {
                Id = Guid.NewGuid(),
                DishId = dishId,
                Amount = 1,
                UserId = userId,
                OrderId = null
            });
            await _context.SaveChangesAsync();
        }
        else
        {
            dishCartEntity.Amount++;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveDishFromCart(Guid dishId, Guid userId)
    {
        //TODO(Накидать Exceptions)
        var dishCartEntity =
            await _context.Carts.Where(x => x.UserId == userId && x.DishId == dishId).FirstOrDefaultAsync();

        if (dishCartEntity == null)
        {
            //TODO(Exception)
            throw new Exception("Exception |_|");
        }

        dishCartEntity.Amount--;
        if (dishCartEntity.Amount == 0)
            _context.Carts.Remove(dishCartEntity);
        await _context.SaveChangesAsync();
    }
}