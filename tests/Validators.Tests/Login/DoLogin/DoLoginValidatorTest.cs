using CashFlow.Application.UseCases.Login.DoLogin;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Login.DoLogin;

public class DoLoginValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestLoginJsonBuilder.Build();

        var result = new DoLoginValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Email = email;

        var result = new DoLoginValidator().Validate(request);

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
        var request = RequestLoginJsonBuilder.Build();
        request.Email = email;

        var result = new DoLoginValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Fact]
    public void Error_Password_Empty()
    {
        var request = RequestLoginJsonBuilder.Build();
        request.Password = string.Empty;

        var result = new DoLoginValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}
