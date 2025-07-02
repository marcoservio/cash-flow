using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository updateRepository, IUnitOfWork unitOfWork, IPasswordEncripter passwordEncripter) : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _updateRepository = updateRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task Execute(RequestChangePasswordJson request)
    {
        var authenticatedUser = await _loggedUser.Get();

        Validate(request, authenticatedUser);

        var user = await _updateRepository.GetById(authenticatedUser.Id);
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _updateRepository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User authenticatedUser)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.Password, authenticatedUser.Password);

        if (!passwordMatch)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(f => f.ErrorMessage)]);
    }
}
