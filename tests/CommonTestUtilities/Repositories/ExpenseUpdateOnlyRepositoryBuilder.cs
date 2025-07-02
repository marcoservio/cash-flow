using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommonTestUtilities.Repositories;

public class ExpenseUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IExpensesUpdateOnlyRepository> _repository;

    public ExpenseUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IExpensesUpdateOnlyRepository>();
    }

    public ExpenseUpdateOnlyRepositoryBuilder GetById(User user, Expense? expense)
    {
        if (expense is not null)
            _repository.Setup(repo => repo.GetById(user, It.IsAny<long>())).ReturnsAsync(expense);

        return this;
    }

    public IExpensesUpdateOnlyRepository Build() => _repository.Object;
}
