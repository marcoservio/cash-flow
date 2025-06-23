using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public UserReadOnlyRepositoryBuilder ExistActiveUserWithEmail(string? email)
    {
        if(!string.IsNullOrWhiteSpace(email))
            _repository.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

        return this;
    }

    public UserReadOnlyRepositoryBuilder GetByEmail(User? user)
    {
        if(user != null)
            _repository.Setup(repo => repo.GetByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
