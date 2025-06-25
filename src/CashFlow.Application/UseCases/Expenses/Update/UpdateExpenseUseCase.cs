using AutoMapper;

using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase(IExpensesUpdateOnlyRepository updateOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser) : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateOnlyRepository _updateOnlyRepository = updateOnlyRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);

        var authenticatedUser = await _loggedUser.Get();

        var expense = await _updateOnlyRepository.GetById(authenticatedUser, id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

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
