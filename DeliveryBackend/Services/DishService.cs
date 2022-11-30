using DeliveryBackend.Data;
using DeliveryBackend.Data.Models.DTO;
using DeliveryBackend.Data.Models.DTO.Queries;
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
            size = 1,
            count = 1,
            current = 1
        };

        return new DishPagedListDto
        {
            dishes = ConvertDishes(dishes),
            paginagion = pagination
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
            throw new Exception("Lol");
        }

        return new DishDto
        {
            category = dishEntity.Category,
            description = dishEntity.Description,
            id = dishEntity.Id,
            image = dishEntity.Image,
            name = dishEntity.Name,
            price = dishEntity.Price,
            //TODO(Высчитывать рейтинг, либо сразу хранить его в бд)
            rating = dishEntity.Price,
            vegetarian = dishEntity.Vegetarian
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
            _context.Ratings.Add(new RatingEntity
            {
                Id = Guid.NewGuid(),
                DishId = id,
                UserId = userId,
                Rating = rating
            });
            await _context.SaveChangesAsync();
        }
        else
        {
            //TODO(Exception)
            throw new Exception("*_*");
        }
    }

    private static List<DishDto?> ConvertDishes(List<DishEntity>? dishes)
    {
        var res = new List<DishDto?>();

        if (dishes == null)
        {
            //TODO(Exception)
            throw new Exception("LoL )_(");
        }

        foreach (var dishEntity in dishes)
        {
            res.Add(new DishDto
            {
                category = dishEntity.Category,
                description = dishEntity.Description,
                id = dishEntity.Id,
                image = dishEntity.Image,
                name = dishEntity.Name,
                price = dishEntity.Price,
                //TODO(Высчитывать рейтинг, либо сразу хранить его в бд)
                rating = dishEntity.Price,
                vegetarian = dishEntity.Vegetarian
            });
        }

        return res;
    }
}