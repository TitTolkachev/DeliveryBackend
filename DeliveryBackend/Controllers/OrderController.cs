using DeliveryBackend.DTO;
using DeliveryBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBackend.Controllers;

[ApiController]
[Route("api/order")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    // [HttpGet]
    // [Authorize]
    // [Route("{id}")]
    // public async Task<OrderDto> GetOrderInfo(Guid id)
    // {
    //     return await _orderService.GetOrderInfo(Guid.Parse(User.Identity.Name));
    // }
    
    // [HttpGet]
    // [Authorize]
    // public async Task<List<OrderInfoDto>> GetOrders()
    // {
    //     return await _orderService.GetOrders(Guid.Parse(User.Identity.Name));
    // }
    
    [HttpPost]
    [Authorize]
    public async Task CreateOrder([FromBody] OrderCreateDto orderCreateDto)
    {
        await _orderService.CreateOrder(Guid.Parse(User.Identity.Name), orderCreateDto);
    }
    
    // [HttpPost]
    // [Authorize]
    // [Route("{id}/status")]
    // public async Task ConfirmOrderDelivery(Guid id)
    // {
    //     await _orderService.ConfirmOrderDelivery(Guid.Parse(User.Identity.Name));
    // }
}