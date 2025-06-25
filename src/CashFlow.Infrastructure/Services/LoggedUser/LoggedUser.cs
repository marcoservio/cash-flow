using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Services.LoggedUser;

public class LoggedUser(CashFlowDbContext context) : ILoggedUser
{
    private readonly CashFlowDbContext _context = context;

    public async Task<User> Get()
    {
        string token = string.Empty;

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Sid)!.Value;

        return await _context.Users
                    .AsNoTracking()
                    .FirstAsync(u => u.UserIdentifier.Equals(Guid.Parse(identifier)));
    }
}
