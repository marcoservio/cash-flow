using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Expenses.Update;

public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var request = RequestExpenseJsonBuilder.Build();

        var useCase = CreateUseCase(user, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        await act.Should().NotThrowAsync();

        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Amount.Should().Be(request.Amount);
        expense.Date.Should().Be(request.Date);
        expense.PaymentType.Should().Be((PaymentType)request.PaymentType);
    }

    [Fact]
    public async Task Error_Title_Required()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user, expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(ex => ex.GetErrors().Count == 1 &&
            ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public async Task Error_Expense_Not_Found()
    {
        var user = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(id: 1000, request);

        (await act.Should().ThrowAsync<NotFoundException>())
            .Where(ex => ex.GetErrors().Count == 1 &&
            ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private static UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryUpdate = new ExpenseUpdateOnlyRepositoryBuilder().GetById(user, expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateExpenseUseCase(repositoryUpdate, unitOfWork, mapper, loggedUser);
    }
}
