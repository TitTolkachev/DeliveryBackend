﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        userRegisterModel.Email = NormalizeAttribute(userRegisterModel.Email);

        await UniqueCheck(userRegisterModel);

        byte[] salt;
        RandomNumberGenerator.Create().GetBytes(salt = new byte[16]);
        var pbkdf2 = new Rfc2898DeriveBytes(userRegisterModel.Password, salt, 100000);
        var hash = pbkdf2.GetBytes(20);
        var hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 16);
        Array.Copy(hash, 0, hashBytes, 16, 20);
        var savedPasswordHash = Convert.ToBase64String(hashBytes);

        await _context.Users.AddAsync(new User
        {
            Id = Guid.NewGuid(),
            FullName = userRegisterModel.FullName,
            BirthDate = userRegisterModel.BirthDate,
            Address = userRegisterModel.Address,
            Email = userRegisterModel.Email,
            Gender = userRegisterModel.Gender,
            Password = savedPasswordHash,
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
        credentials.Email = NormalizeAttribute(credentials.Email);

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

    public async Task Logout(string token)
    {
        var alreadyExistsToken = await _context.Tokens.FirstOrDefaultAsync(x => x.InvalidToken == token);

        if (alreadyExistsToken == null)
        {
            var handler = new JwtSecurityTokenHandler();
            var expiredDate = handler.ReadJwtToken(token).ValidTo;
            _context.Tokens.Add(new Token { InvalidToken = token, ExpiredDate = expiredDate });
            await _context.SaveChangesAsync();
        }
        else
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
                "Token is already invalid"
            );
            throw ex;
        }
    }

    public async Task<UserDto> GetUserProfile(Guid userId)
    {
        var userEntity = await _context
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (userEntity != null)
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
        
        var ex = new Exception();
        ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
            "User not exists"
        );
        throw ex;
    }


    public async Task EditUserProfile(Guid userId, UserEditModel userEditModel)
    {
        var userEntity = await _context
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (userEntity == null)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
                "User not exists"
            );
            throw ex;
        }

        //TODO (Раскомментировать, когда переделаю енамы)
        // if (userEditModel.Gender != "Male" && userEditModel.Gender != "Female")
        // {
        //     var ex = new Exception();
        //     ex.Data.Add(StatusCodes.Status400BadRequest.ToString(),
        //         "Invalid Gender"
        //     );
        //     throw ex;
        // }

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
            .FirstOrDefaultAsync(x => x.Email == email);

        if (userEntity == null)
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
                "User not exists"
            );
            throw ex;
        }

        if (!CheckHashPassword(userEntity.Password, password))
        {
            var ex = new Exception();
            ex.Data.Add(StatusCodes.Status401Unauthorized.ToString(),
                "Wrong password"
            );
            throw ex;
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

    private static bool CheckHashPassword(string savedPasswordHash, string password)
    {
        var hashBytes = Convert.FromBase64String(savedPasswordHash);
        var salt = new byte[16];
        Array.Copy(hashBytes, 0, salt, 0, 16);
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
        var hash = pbkdf2.GetBytes(20);
        for (var i = 0; i < 20; i++)
            if (hashBytes[i + 16] != hash[i])
                return false;
        return true;
    }

    private static string NormalizeAttribute(string value)
    {
        return value.ToLower().TrimEnd();
    }

    private async Task UniqueCheck(UserRegisterModel userRegisterModel)
    {
        var email = await _context
            .Users
            .Where(x => userRegisterModel.Email == x.Email)
            .FirstOrDefaultAsync();

        if (email != null)
        {
            var ex = new Exception(
                $"{StatusCodes.Status409Conflict} - Account with email '{userRegisterModel.Email}' already exists");
            ex.Data.Add(StatusCodes.Status409Conflict.ToString(),
                $"Account with email '{userRegisterModel.Email}' already exists"
            );
            throw ex;
        }
    }
}