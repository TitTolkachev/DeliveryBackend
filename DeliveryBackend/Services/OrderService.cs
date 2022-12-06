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

    public async Task<OrderDto> GetOrderInfo(Guid userId, Guid orderId)
    {
        var orderInfo = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
        var orderCarts = await _context.Carts.Where(x => x.OrderId == orderId).ToListAsync();

        var dishes = new List<Dish>();
        foreach (var orderCart in orderCarts)
        {
            var dish = await _context.Dishes.FirstOrDefaultAsync(x => x.Id == orderCart.DishId);
            //TODO(Exception)
            if (dish != null)
                dishes.Add(dish);
        }
        
        //TODO(Exception)
        if (orderInfo == null)
            throw new Exception("Order Info not found");

        var convertedDishes = (from orderCart in orderCarts
            let dish = dishes.FirstOrDefault(x => x.Id == orderCart.DishId)
            where dish != null
            select new DishBasketDto
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                TotalPrice = orderCart.Amount * dish.Price,
                Amount = orderCart.Amount,
                Image = dish.Image
            }).ToList();

        //TODO(Exception)
        if (convertedDishes == null)
            throw new Exception("Empty order list returned");

        return new OrderDto
        {
            Id = orderInfo.Id,
            DeliveryTime = orderInfo.DeliveryTime,
            OrderTime = orderInfo.OrderTime,
            Status = orderInfo.Status,
            Price = orderInfo.Price,
            Dishes = convertedDishes,
            Address = orderInfo.Address
        };
    }

    public async Task<List<OrderInfoDto>> GetOrders(Guid userId)
    {
        var orders = await _context.Orders.Where(x => x.UserId == userId).ToListAsync();

        return _mapper.Map<List<OrderInfoDto>>(orders);
    }

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

    public async Task ConfirmOrderDelivery(Guid userId, Guid orderId)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == orderId);

        //TODO(Exception)
        if (order == null)
            throw new Exception("Order not found");

        //TODO(Exception)
        if (order.UserId != userId)
            throw new Exception("UserId is not satisfied");

        order.Status = OrderStatus.Delivered;
        await _context.SaveChangesAsync();
    }

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