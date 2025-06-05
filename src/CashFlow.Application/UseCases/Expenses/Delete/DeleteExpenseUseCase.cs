using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(IExpensesWriteOnlyRepository expensesWriteOnlyRepository, IUnitOfWork unitOfWork) : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id)
    {
        var result = _expensesWriteOnlyRepository.Delete(id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);            

        await _unitOfWork.Commit();
    }
}
