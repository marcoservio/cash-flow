using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.LoggedUser;
using FluentAssertions;
using CommonTestUtilities.Entities;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();

        var request = RequestExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().NotBeNullOrEmpty();
        result.Title.Should().Be(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var user = UserBuilder.Build();

        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(ex => ex.GetErrors().Count == 1 &&
                ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    private static RegisterExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
    {
        var writeRepository = ExpensesWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(writeRepository, unitOfWork, mapper, loggedUser);
    }
}
