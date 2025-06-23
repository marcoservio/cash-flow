using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login.DoLogin;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrEmpty();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Email_Empty()
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Email = string.Empty;

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<ErrorOnValidationException>())
            .Where(ex => ex.GetErrors().Count == 1 &&
                ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = RequestLoginJsonBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<InvalidLoginException>())
            .WithMessage(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
    }

    [Fact]
    public async Task Error_Password_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        (await act.Should().ThrowAsync<InvalidLoginException>())
            .WithMessage(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
    }

    private static DoLoginUseCase CreateUseCase(User? user = null, string? password = null)
    {
        var readRepository = new UserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();
        var tokenGenerator = JwtTokenGenerationBuilder.Build();

        return new DoLoginUseCase(readRepository, passwordEncripter, tokenGenerator);
    }
}
