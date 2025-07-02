using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetById;

public class GetExpenseByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user, expense);

        var result = await useCase.Execute(expense.Id);

        result.Should().NotBeNull();
        result.Id.Should().Be(expense.Id);
        result.Title.Should().Be(expense.Title);
        result.Description.Should().Be(expense.Description);
        result.Amount.Should().Be(expense.Amount);
        result.Date.Should().Be(expense.Date);
        result.PaymentType.Should().Be((PaymentType)expense.PaymentType);
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(id: 1000);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(ex => ex.GetErrors().Count == 1 &&
            ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private static GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetExpenseByIdUseCase(repository, mapper, loggedUser);
    }
}
