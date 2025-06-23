using Bogus;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(r => r.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password(prefix: "!Aa1"));
    }
}
