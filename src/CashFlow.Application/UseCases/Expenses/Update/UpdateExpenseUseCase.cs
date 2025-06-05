using AutoMapper;

using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase(IExpensesUpdateOnlyRepository updateOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper) : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var expense = await _updateOnlyRepository.GetById(id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);

        _updateOnlyRepository.Update(expense);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestExpenseJson request)
    {
        var result = new ExpenseValidator().Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(f => f.ErrorMessage)]);
    }
}
