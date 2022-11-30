using DeliveryBackend.Data.Models.DTO;
using DeliveryBackend.Data.Models.DTO.Queries;
using DeliveryBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBackend.Controllers;

[ApiController]
[Route("api/dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpGet]
    public async Task<DishPagedListDto> GetDishList([FromQuery] GetDishListQuery dishListQuery)
    {
        return await _dishService.GetDishList(dishListQuery);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<DishDto> GetDish(Guid id)
    {
        return await _dishService.GetDish(id);
    }

    [HttpGet]
    [Authorize]
    [Route("{id}/rating/check")]
    public async Task<bool> CheckDishRating(Guid id)
    {
        return await _dishService.CheckDishRating(id, Guid.Parse(User.Identity.Name));
    }

    [HttpPost]
    [Authorize]
    [Route("{id}/rating")]
    public async Task SetDishRating(Guid id, [FromQuery] int rating)
    {
        await _dishService.SetDishRating(id, rating, Guid.Parse(User.Identity.Name));
    }
}