using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class ExpenseBuilder
{
    public static List<Expense> Collection(User user, uint count = 2)
    {
        var list = new List<Expense>();

        if (count == 0)
            count = 1;

        var expenseId = 1;

        for (int i = 0; i < count; i++)
        {
            var expense = Build(user);
            expense.Id = expenseId++;

            list.Add(expense);
        }

        return list;
    }

    public static Expense Build(User user)
    {
        return new Faker<Expense>()
            .RuleFor(e => e.Id, 1)
            .RuleFor(e => e.Title, f => f.Commerce.ProductName())
            .RuleFor(e => e.Description, f => f.Commerce.ProductDescription())
            .RuleFor(e => e.Amount, f => f.Finance.Amount(1, 1000))
            .RuleFor(e => e.Date, f => f.Date.Past())
            .RuleFor(e => e.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(e => e.UserId, user.Id)
            .RuleFor(e => e.Tags, f => f.Make(1, () => new CashFlow.Domain.Entities.Tag
            {
                Id = 1,
                Value = f.PickRandom<CashFlow.Domain.Enums.Tag>(),
                ExpenseId = 1
            }));
    }
}
