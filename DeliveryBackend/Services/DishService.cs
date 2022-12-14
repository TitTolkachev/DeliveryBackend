using AutoMapper;
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
    private readonly IMapper _mapper;

    public DishService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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
                Dishes = _mapper.Map<List<DishDto>>(dishes),
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
            return _mapper.Map<DishDto>(dishEntity);

        var ex = new Exception();
        ex.Data.Add(StatusCodes.Status404NotFound.ToString(),
            "Dish entity not found"
        );
        throw ex;
    }

    public async Task<bool> CheckDishRating(Guid id, Guid userId)
    {
        await CheckDishInDb(id);

        var ratingEntity = await _context.Ratings.FirstOrDefaultAsync(x => x.DishId == id && x.UserId == userId);
        return ratingEntity == null;
    }

    public async Task SetDishRating(Guid id, int rating, Guid userId)
    {
        CheckRating(rating);
        await CheckDishInDb(id);

        if (await CheckDishRating(id, userId))
        {
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

    private async Task CheckDishInDb(Guid dishId)
    {
        if (await _context.Dishes.FirstOrDefaultAsync(x => x.Id == dishId) == null)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status404NotFound.ToString(),
                "Dish entity not found"
            );
            throw ex;
        }
    }

    private static void CheckRating(int rating)
    {
        if (rating is >= 0 and <= 10) return;
        var e = new Exception();
        e.Data.Add(StatusCodes.Status400BadRequest.ToString(),
            "Bad Request, Rating range is 0-10"
        );
        throw e;
    }
}