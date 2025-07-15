﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Services.LoggedUser;

public class LoggedUser(CashFlowDbContext context, ITokenProvider tokenProvider) : ILoggedUser
{
    private readonly CashFlowDbContext _context = context;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<User> Get()
    {
        string token = _tokenProvider.TokenOnRequest();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        return await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserIdentifier.Equals(Guid.Parse(identifier))) ??
                    throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
    }
}
