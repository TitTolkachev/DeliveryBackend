using AutoMapper;
using DeliveryBackend.Data;
using DeliveryBackend.DTO;
using DeliveryBackend.DTO.Queries;
using DeliveryBackend.Data.Models.Entities;
using DeliveryBackend.Data.Models.Enums;
using DeliveryBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        var dishList = await _context.Dishes.Where(x =>
            dishListQuery.Categories.Contains(x.Category) &&
            dishListQuery.Vegetarian == x.Vegetarian).ToListAsync();

        var dishesOrdered = dishListQuery.Sorting switch
        {
            DishSorting.NameAsc => dishList.OrderBy(s => s.Name),
            DishSorting.NameDesc => dishList.OrderByDescending(s => s.Name),
            DishSorting.PriceAsc => dishList.OrderBy(s => s.Price),
            DishSorting.PriceDesc => dishList.OrderByDescending(s => s.Price),
            DishSorting.RatingAsc => dishList.OrderBy(s => s.Rating),
            DishSorting.RatingDesc => dishList.OrderByDescending(s => s.Rating),
            _ => dishList.OrderBy(s => s.Name)
        };
        var dishes = dishesOrdered.Skip((dishListQuery.Page - 1) * 5).Take(Range.EndAt(5)) .ToList();

        var pagination = new PageInfoModel
        {
            Size = dishes.Count,
            Count = dishList.Count,
            Current = dishListQuery.Page
        };

        if (pagination.Current <= pagination.Count && pagination.Current > 0)
            return new DishPagedListDto
            {
                Dishes = _mapper.Map<List<DishDto>>(dishes),
                Pagination = pagination
            };

        var ex = new Exception();
        ex.Data.Add(StatusCodes.Status400BadRequest.ToString(),
            "Invalid value for attribute page"
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

            var dishEntity = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == id);
            var dishRatingList = await _context.Ratings.Where(x => x.DishId == id).ToListAsync();
            var sum = dishRatingList.Sum(r => r.RatingScore);
            dishEntity!.Rating = sum / dishRatingList.Count;

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