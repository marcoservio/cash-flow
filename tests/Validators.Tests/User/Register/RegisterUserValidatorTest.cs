using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.User.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = new  RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name!;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string? email)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email!;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Theory]
    [InlineData("test@")]
    [InlineData("test")]
    [InlineData("teste.com")]
    [InlineData("12321")]
    [InlineData("@.com")]
    public void Error_Email_Invalid(string email)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Password_Empty(string? password)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = password!;

        var result = new RegisterUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}
