using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens.Generator;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
