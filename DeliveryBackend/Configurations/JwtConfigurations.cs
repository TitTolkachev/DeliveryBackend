﻿using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryBackend.Configurations;

public class JwtConfigurations
{
    public const string Issuer = "DeliveryBackendDevelop";
    public const string Audience = "DeliveryFronted";
    private const string Key = "Le0n228HotM0nk1yLol321H0wToKakAt";
    public const int Lifetime = 30;

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}