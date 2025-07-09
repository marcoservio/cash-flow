using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete
{
    public class DeleteUserAccountUseCase(ILoggedUser loggedUser, IUserWriteOnlyRepository writeOnlyRepository, IUnitOfWork unitOfWork) : IDeleteUserAccountUseCase
    {
        private readonly ILoggedUser _loggedUser = loggedUser;
        private readonly IUserWriteOnlyRepository writeOnlyRepository = writeOnlyRepository;
        private readonly IUnitOfWork unitOfWork = unitOfWork;

        public async Task Execute()
        {
            var authenticatedUserId = await _loggedUser.Get();

            await writeOnlyRepository.Delete(authenticatedUserId);

            await unitOfWork.Commit();
        }
    }
}
