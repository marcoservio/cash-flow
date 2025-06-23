using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Cryptography;

public class PasswordEncrypterBuilder
{
    private Mock<IPasswordEncripter> _mock;

    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncripter>();

        _mock.Setup(x => x.Encrypt(It.IsAny<string>()))
            .Returns("djsakldjalkdjkasldjlkasjdls");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if(!string.IsNullOrWhiteSpace(password))
            _mock.Setup(x => x.Verify(password, It.IsAny<string>())).Returns(true);

        return this;
    }

    public IPasswordEncripter Build() => _mock.Object;
}
