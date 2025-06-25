using AutoMapper;

using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase(IExpensesReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser) : IGetAllExpensesUseCase
{
    private readonly IExpensesReadOnlyRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<ResponseExpensesJson> Execute()
    {
        var authenticatedUser = await _loggedUser.Get();

        var result = await _repository.GetAll(authenticatedUser);

        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}
