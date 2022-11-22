using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBackend.Controllers;

[ApiController]
[Route("api/basket")]
public class BasketController : ControllerBase
{
    // private readonly IBasketService _basketService;
    //
    // public BasketController(IBasketService basketService)
    // {
    //     _basketService = basketService;
    // }
    //
    // [HttpGet]
    // [Authorize]
    // [Authorize(Policy = "ValidateToken")]
    // public async Task<DishBasketDto> GetUserCart()
    // {
    //     return await _basketService.GetUserCart();
    // }
    //
    // [HttpPost]
    // [Authorize]
    // [Authorize(Policy = "ValidateToken")]
    // [Route("dish/{dishId}")]
    // public async Task AddDishToCart(Guid dishId)
    // {
    //     await _basketService.AddDishToCart(dishId);
    // }
    //
    // [HttpDelete]
    // [Authorize]
    // [Authorize(Policy = "ValidateToken")]
    // [Route("dish/{dishId}")]
    // public async Task DecreaseDishQuantityInCart(Guid dishId)
    // {
    //     await _basketService.AddDishToCart(dishId);
    // }
}