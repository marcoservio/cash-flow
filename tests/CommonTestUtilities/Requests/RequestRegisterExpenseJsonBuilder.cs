using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterExpenseJsonBuilder
{
    public static RequestExpenseJson Build()
    {
        return new Faker<RequestExpenseJson>()
            .RuleFor(e => e.Title, f => f.Lorem.Word())
            .RuleFor(e => e.Description, f => f.Lorem.Paragraph(1))
            .RuleFor(e => e.Amount, f => Convert.ToDecimal(f.Commerce.Price()))
            .RuleFor(e => e.PaymentType, f => f.PickRandom<PaymentType>())
            .RuleFor(e => e.Date, f => f.Date.Past());
    }
}
