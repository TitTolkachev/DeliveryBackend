using AutoMapper;
using DeliveryBackend.Configurations;
using DeliveryBackend.Data;
using DeliveryBackend.Mappings;
using DeliveryBackend.Services;
using DeliveryBackend.Services.Interfaces;
using DeliveryBackend.Services.ValidateTokenPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery.Api",
        Version = "v1",
        Description = "The API for my application"
    });
});

// DB
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connection));

// AutoMapping
var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddMvc();

// Services
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Auth
builder.Services.AddSingleton<IAuthorizationHandler, ValidateTokenRequirementHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "ValidateToken",
        policy => policy.Requirements.Add(new ValidateTokenRequirement()));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = JwtConfigurations.Issuer,
            ValidateAudience = true,
            ValidAudience = JwtConfigurations.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = JwtConfigurations.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };
    });

// Build App
var app = builder.Build();

// Auto Migration
using var serviceScope = app.Services.CreateScope();
var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
context?.Database.Migrate();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();