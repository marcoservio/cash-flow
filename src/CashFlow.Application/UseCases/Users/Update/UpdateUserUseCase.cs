using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Update;

public class UpdateUserUseCase(ILoggedUser loggedUser, IUserUpdateOnlyRepository updateRepositoy, IUserReadOnlyRepository readRepository, IUnitOfWork unitOfWork) : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUserUpdateOnlyRepository _updateRepositoy = updateRepositoy;
    private readonly IUserReadOnlyRepository _readRepository = readRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(RequestUpdateUserJson request)
    {
        var authenticatedUser = await _loggedUser.Get();

        await Validate(request, authenticatedUser.Email);

        var user = await _updateRepositoy.GetById(authenticatedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _updateRepositoy.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

        if (!currentEmail.Equals(request.Email))
        {
            var userExists = await _readRepository.ExistActiveUserWithEmail(request.Email);

            if (userExists)
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
            throw new ErrorOnValidationException([.. result.Errors.Select(e => e.ErrorMessage)]);
    }
}
