using DeliveryBackend.Data.Models;

namespace DeliveryBackend.Services.Interfaces;

public interface IUsersService
{
    Task<TokenResponse> RegisterUser(UserRegisterModel userRegisterDto);
    Task<TokenResponse> LoginUser( LoginCredentials credentials);
}