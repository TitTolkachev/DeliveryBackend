using System.Text.RegularExpressions;
using DeliveryBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace DeliveryBackend.Services.ValidateTokenPolicy;

public class ValidateTokenRequirementHandler : AuthorizationHandler<ValidateTokenRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public ValidateTokenRequirementHandler(IHttpContextAccessor httpContextAccessor,
        IServiceScopeFactory serviceScopeFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ValidateTokenRequirement requirement)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var authorizationString = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
            var token = GetToken(authorizationString);

            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            var tokenEntity = await dbContext
                .Tokens
                .Where(x => x.InvalidToken == token)
                .FirstOrDefaultAsync();
            
            if (tokenEntity != null)
            {
                //TODO(Exception)
                throw new Exception("Not authorized");
            }

            context.Succeed(requirement);
        }
        else
        {
            //TODO(Exception)
            throw new Exception("Bad request");
        }
    }

    private static string GetToken(string authorizationString)
    {
        const string pattern = @"\S+\.\S+\.\S+";
        var regex = new Regex(pattern);
        var matches = regex.Matches(authorizationString);

        if (matches.Count <= 0)
        {
            //TODO(Exception)
            throw new Exception("Not authorized");
        }

        var token = matches[0].Value;

        if (token == null)
        {
            //TODO(Exception)
            throw new Exception("Not authorized");
        }

        return token;
    }
}