using DeliveryBackend.Data.Models.DTO;
using DeliveryBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBackend.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;
    
    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<List<DishBasketDto>> GetUserCart()
    {
        return await _basketService.GetUserCart(Guid.Parse(User.Identity.Name));
    }
    
    [HttpPost]
    [Authorize]
    [Route("dish/{dishId}")]
    public async Task AddDishToCart(Guid dishId)
    {
        await _basketService.AddDishToCart(dishId, Guid.Parse(User.Identity.Name));
    }
    
    [HttpDelete]
    [Authorize]
    [Route("dish/{dishId}")]
    public async Task DecreaseDishQuantityInCart(Guid dishId)
    {
        await _basketService.RemoveDishFromCart(dishId, Guid.Parse(User.Identity.Name));
    }
}