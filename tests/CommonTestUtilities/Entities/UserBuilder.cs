using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Cryptography;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncripter = new PasswordEncrypterBuilder().Build();

        return new Faker<User>()
            .RuleFor(u => u.Id, f => 1)
            .RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
            .RuleFor(u => u.Password, (f, u) => f.Internet.Password(prefix: "!Aa1"))
            .RuleFor(u => u.Role, role);
    }
}
