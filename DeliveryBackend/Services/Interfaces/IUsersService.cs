using DeliveryBackend.Data.Models.DTO;

namespace DeliveryBackend.Services.Interfaces;

public interface IUsersService
{
    Task<TokenResponse> RegisterUser(UserRegisterModel userRegisterDto);
    Task<TokenResponse> LoginUser( LoginCredentials credentials);
    Task<UserDto> GetUserProfile( Guid userId);
    Task EditUserProfile(Guid userId, UserEditModel userEditModel);
}