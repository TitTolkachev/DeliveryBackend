using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeliveryBackend.Configurations;
using DeliveryBackend.Data;
using DeliveryBackend.Data.Models;
using DeliveryBackend.Data.Models.Entities;
using DeliveryBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryBackend.Services;

public class UsersService : IUsersService
{
    private readonly ApplicationDbContext _context;

    public UsersService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TokenResponse> RegisterUser(UserRegisterModel userRegisterModel)
    {
        //TODO (сделать нормализацию входных данных)
        //userRegisterModel.email = NormalizeAttribute(userRegisterModel.email);
        //userRegisterModel.fullName = NormalizeAttribute(userRegisterModel.fullName);
        
        
        //TODO (сделать проверку на уникальность входных данных)
        
        await _context.Users.AddAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            FullName = userRegisterModel.fullName,
            BirthDate = userRegisterModel.birthDate,
            Address = userRegisterModel.address,
            Email = userRegisterModel.email,
            Gender = userRegisterModel.gender,
            Password = userRegisterModel.password,
            PhoneNumber = userRegisterModel.phoneNumber,
            
        });
        await _context.SaveChangesAsync();
        
        var credentials = new LoginCredentials
        {
            email = userRegisterModel.email,
            password = userRegisterModel.password
        };
        
        return await LoginUser(credentials);
    }

    public async Task<TokenResponse> LoginUser(LoginCredentials credentials)
    {
        //TODO (normilize)
        //credentials.username = NormalizeAttribute(credentials.email);

        var identity = await GetIdentity(credentials.email, credentials.password);

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: JwtConfigurations.Issuer,
            audience: JwtConfigurations.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.AddMinutes(JwtConfigurations.Lifetime),
            signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodeJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var result = new TokenResponse()
        {
            token = encodeJwt
        };

        return result;
    }
    
    private async Task<ClaimsIdentity> GetIdentity(string email, string password)
    {
        var userEntity = await _context
            .Users
            .Where(x => x.Email == email && x.Password == password)
            .FirstOrDefaultAsync();

        if (userEntity == null)
        {
            //TODO (сделать свой эксцепшн)
            throw new Exception("Login failed");
        }

        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, userEntity.Id.ToString())
        };

        var claimsIdentity = new ClaimsIdentity
        (
            claims,
            "Token",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );

        return claimsIdentity;
    }
}