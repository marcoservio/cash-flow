﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens.Generator;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infrastructure.Security.Tokens.Generator;

public class JwtTokenGenerator(uint expirationTimeInMinutes, string signingKey) : IAccessTokenGenerator
{
    private readonly uint _expirationTimeInMinutes = expirationTimeInMinutes;
    private readonly string _signingKey = signingKey;

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Sid, user.UserIdentifier.ToString()),
            new(ClaimTypes.Role, user.Role),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
            Subject = new ClaimsIdentity(claims),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey() => new(Encoding.UTF8.GetBytes(_signingKey));
}
