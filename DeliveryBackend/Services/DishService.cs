using DeliveryBackend.Data;
using DeliveryBackend.DTO;
using DeliveryBackend.DTO.Queries;
using DeliveryBackend.Data.Models.Entities;
using DeliveryBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryBackend.Services;

public class DishService : IDishService
{
    private readonly ApplicationDbContext _context;

    public DishService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DishPagedListDto> GetDishList(GetDishListQuery dishListQuery)
    {
        //TODO(Сделать так, чтобы возвращалась по параметрам из запроса, а не все подряд)
        var dishes = await _context.Dishes.ToListAsync();

        //TODO(в паганиции возвращать тоже адекватные значения)
        var pagination = new PageInfoModel
        {
            Size = 1,
            Count = 1,
            Current = 1
        };

        return new DishPagedListDto
        {
            Dishes = ConvertDishes(dishes),
            Pagination = pagination
        };
    }

    public async Task<DishDto> GetDish(Guid id)
    {
        var dishEntity = await _context.Dishes
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        //TODO(Exception)
        if (dishEntity == null)
        {
            throw new KeyNotFoundException("Lol");
        }

        return new DishDto
        {
            Category = dishEntity.Category,
            Description = dishEntity.Description,
            Id = dishEntity.Id,
            Image = dishEntity.Image,
            Name = dishEntity.Name,
            Price = dishEntity.Price,
            //TODO(Высчитывать рейтинг, либо сразу хранить его в бд)
            Rating = dishEntity.Price,
            Vegetarian = dishEntity.Vegetarian
        };
    }

    public async Task<bool> CheckDishRating(Guid id, Guid userId)
    {
        //TODO(Слелать проверку на коррентные id и рейтинг)
        var ratingEntity = await _context.Ratings.FirstOrDefaultAsync(x => x.DishId == id && x.UserId == userId);
        return ratingEntity == null;
    }
    
    public async Task SetDishRating(Guid id, int rating, Guid userId)
    {
        var ratingEntity = await _context.Ratings.FirstOrDefaultAsync(x => x.DishId == id && x.UserId == userId);
        if (ratingEntity == null)
        {
            //TODO(Слелать проверку на коррентные id и рейтинг)
            _context.Ratings.Add(new Rating
            {
                Id = Guid.NewGuid(),
                DishId = id,
                UserId = userId,
                RatingScore = rating
            });
            await _context.SaveChangesAsync();
        }
        else
        {
            //TODO(Exception)
            throw new Exception("*_*");
        }
    }

    private static List<DishDto> ConvertDishes(List<Dish> dishes)
    {
        if (dishes == null)
        {
            //TODO(Exception)
            throw new Exception("LoL )_(");
        }

        return dishes.Select(dishEntity => new DishDto
            {
                Category = dishEntity.Category,
                Description = dishEntity.Description,
                Id = dishEntity.Id,
                Image = dishEntity.Image,
                Name = dishEntity.Name,
                Price = dishEntity.Price,
                //TODO(Высчитывать рейтинг, либо сразу хранить его в бд)
                Rating = dishEntity.Price,
                Vegetarian = dishEntity.Vegetarian
            })
            .ToList();
    }
}