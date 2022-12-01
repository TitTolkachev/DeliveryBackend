using AutoMapper;
using DeliveryBackend.Data;
using DeliveryBackend.Data.Models.Entities;
using DeliveryBackend.Data.Models.Enums;
using DeliveryBackend.DTO;
using DeliveryBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryBackend.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public OrderService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // public Task<OrderDto> GetOrderInfo(Guid userId)
    // {
    //     
    // }
    //
    // public Task<List<OrderInfoDto>> GetOrders(Guid userId)
    // {
    //     
    // }

    public async Task CreateOrder(Guid userId, OrderCreateDto orderCreateDto)
    {
        //TODO(Сделать проверку пришедшей дто)

        
        //TODO(Проверка на то, есть ли что-то в корзине. Сейчас она в CreateOrderOperations)
        // В бд всем товарам в корзине проставить, что теперь они в заказе, заодно посчитать стоимость
        var orderId = Guid.NewGuid();
        var newOrder = new Order
        {
            Id = orderId,
            DeliveryTime = orderCreateDto.DeliveryTime,
            OrderTime = DateTime.UtcNow,
            Status = OrderStatus.InProcess,
            Price = 0,
            Address = orderCreateDto.Address,
            UserId = userId
        };
        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();
        newOrder.Price = await CreateOrderOperations(orderId, userId);
        await _context.SaveChangesAsync();
    }

    // public Task ConfirmOrderDelivery(Guid userId)
    // {
    //     
    // }

    private async Task<double> CreateOrderOperations(Guid orderId, Guid userId)
    {
        double res = 0;
        var cartDishes = await _context.Carts
            .Where(x => x.UserId == userId && x.OrderId == null)
            .ToListAsync();

        //TODO(Exception)
        if (cartDishes.IsNullOrEmpty())
        {
            throw new Exception("Cart Not Found");
        }

        for (var i = 0; i < cartDishes.Count; i++)
        {
            cartDishes[i].OrderId = orderId;
            var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == cartDishes[i].DishId);
            if (dish == null)
            {
                //TODO(Exception)
                throw new Exception("()_()");
            }

            res += cartDishes[i].Amount * dish.Price;
        }
        await _context.SaveChangesAsync();

        return res;
    }
}