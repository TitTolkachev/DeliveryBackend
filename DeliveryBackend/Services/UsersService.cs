using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DeliveryBackend.Configurations;
using DeliveryBackend.Data;
using DeliveryBackend.DTO;
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

        await _context.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            FullName = userRegisterModel.FullName,
            BirthDate = userRegisterModel.BirthDate,
            Address = userRegisterModel.Address,
            Email = userRegisterModel.Email,
            Gender = userRegisterModel.Gender,
            Password = userRegisterModel.Password,
            PhoneNumber = userRegisterModel.PhoneNumber,
        });
        await _context.SaveChangesAsync();

        var credentials = new LoginCredentials
        {
            Email = userRegisterModel.Email,
            Password = userRegisterModel.Password
        };

        return await LoginUser(credentials);
    }

    public async Task<TokenResponse> LoginUser(LoginCredentials credentials)
    {
        //TODO (normilize)
        //credentials.username = NormalizeAttribute(credentials.email);

        var identity = await GetIdentity(credentials.Email, credentials.Password);

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
            Token = encodeJwt
        };

        return result;
    }

    public async Task<UserDto> GetUserProfile(Guid userId)
    {
        var userEntity = await _context
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (userEntity == null)
        {
            //TODO(сделать эксцепшн)
            throw new Exception("Lol, you're not found -_-");
        }

        return new UserDto
        {
            Id = userEntity.Id,
            FullName = userEntity.FullName,
            BirthDate = userEntity.BirthDate,
            Gender = userEntity.Gender,
            Address = userEntity.Address,
            Email = userEntity.Email,
            PhoneNumber = userEntity.PhoneNumber
        };
    }


    public async Task EditUserProfile(Guid userId, UserEditModel userEditModel)
    {
        var userEntity = await _context
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (userEntity == null)
        {
            //TODO(сделать эксцепшн)
            throw new Exception("Lol, you're not found -_-");
        }
        
        //TODO (сделать все остальные проверки на входные данные)

        userEntity.FullName = userEditModel.FullName;
        userEntity.BirthDate = userEditModel.BirthDate;
        userEntity.Address = userEditModel.Address;
        userEntity.Gender = userEditModel.Gender;
        userEntity.PhoneNumber = userEditModel.PhoneNumber;
        
        await _context.SaveChangesAsync();
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
            new(ClaimsIdentity.DefaultNameClaimType, userEntity.Id.ToString())
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