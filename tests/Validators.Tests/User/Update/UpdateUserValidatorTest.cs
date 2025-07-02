using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.User.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name!;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string? email)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email!;

        var result = new UpdateUserValidator().Validate(request);

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
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}
