using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.NewPassword, f => f.Internet.Password(prefix: "!Aa1"));
    }
}
