using AutoMapper;

using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase(IExpensesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser) : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var authenticatedUser = await _loggedUser.Get();

        var result = await _repository.GetById(authenticatedUser, id) ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        return _mapper.Map<ResponseExpenseJson>(result);
    }
}
