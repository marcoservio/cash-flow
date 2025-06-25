using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(IExpensesWriteOnlyRepository expensesWriteOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser, IExpensesReadOnlyRepository expensesReadOnlyRepository) : IDeleteExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository = expensesReadOnlyRepository;
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task Execute(long id)
    {
        var authenticatedUser = await _loggedUser.Get();

        _ = await _expensesReadOnlyRepository.GetById(authenticatedUser, id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _expensesWriteOnlyRepository.Delete(id);

        await _unitOfWork.Commit();
    }
}
