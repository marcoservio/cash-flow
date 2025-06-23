using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build() => new Mock<IUserWriteOnlyRepository>().Object;
}
