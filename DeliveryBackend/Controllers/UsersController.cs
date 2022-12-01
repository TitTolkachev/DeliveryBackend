using DeliveryBackend.DTO;
using DeliveryBackend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryBackend.Controllers;

[ApiController]
[Route("api/account")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<TokenResponse> RegisterUser([FromBody] UserRegisterModel userRegisterDto)
    {
        return await _usersService.RegisterUser(userRegisterDto);
    }

    [HttpPost]
    [Route("login")]
    public async Task<TokenResponse> Login([FromBody] LoginCredentials credentials)
    {
        return await _usersService.LoginUser(credentials);
    }


    [HttpGet]
    [Authorize]
    [Route("profile")]
    public async Task<UserDto> GetUserProfile()
    {
        return await _usersService.GetUserProfile(
            Guid.Parse(User.Identity.Name));
    }

    [HttpPut]
    [Authorize]
    [Route("profile")]
    public async Task EditUserProfile([FromBody] UserEditModel userEditModel)
    {
        await _usersService.EditUserProfile(
            Guid.Parse(User.Identity.Name), userEditModel);
    }
}