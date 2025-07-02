using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user);

        var useCase = CreateUseCase(user, expense);

        var act = async () => await useCase.Execute(expense.Id);

        await act.Should().NotThrowAsync();
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

    private static DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repositoryWrite = ExpensesWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryRead = new ExpensesReadOnlyRepositoryBuilder().GetById(user, expense).Build();

        return new DeleteExpenseUseCase(repositoryWrite, unitOfWork, loggedUser, repositoryRead);
    }
}
