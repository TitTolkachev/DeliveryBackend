using DeliveryBackend.Data;
using DeliveryBackend.DTO;
using DeliveryBackend.DTO.Queries;
using DeliveryBackend.Data.Models.Entities;
using DeliveryBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

        if (!dishes.IsNullOrEmpty())
            return new DishPagedListDto
            {
                Dishes = ConvertDishes(dishes),
                Pagination = pagination
            };

        var ex = new Exception();
        ex.Data.Add(StatusCodes.Status404NotFound.ToString(),
            "Page not found"
        );
        throw ex;
    }

    public async Task<DishDto> GetDish(Guid id)
    {
        var dishEntity = await _context.Dishes
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (dishEntity != null)
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

        var ex = new Exception();
        ex.Data.Add(StatusCodes.Status404NotFound.ToString(),
            "Dish entity not found"
        );
        throw ex;
    }

    public async Task<bool> CheckDishRating(Guid id, Guid userId)
    {
        //TODO(Слелать проверку на коррентные id и рейтинг)
        var ratingEntity = await _context.Ratings.FirstOrDefaultAsync(x => x.DishId == id && x.UserId == userId);
        return ratingEntity == null;
    }

    public async Task SetDishRating(Guid id, int rating, Guid userId)
    {
        if (await CheckDishRating(id, userId))
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

        var ex = new Exception();
        ex.Data.Add(StatusCodes.Status409Conflict.ToString(),
            "Rating entity already exists"
        );
        throw ex;
    }

    private static List<DishDto> ConvertDishes(IEnumerable<Dish> dishes)
    {
        //TODO(Скорее всего можно заавтомаппить)
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