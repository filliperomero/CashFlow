using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Security;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncrypter> _mock;
    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncrypter>();

        _mock
            .Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>()))
            .Returns("!Password-encrypted-123");
    }

    public PasswordEncrypterBuilder Verify(string? password)
    {
        if (string.IsNullOrWhiteSpace(password) is false)
        {
            _mock
                .Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>()))
                .Returns(true);
        }
        
        return this;
    }

    public IPasswordEncrypter Build() => _mock.Object;
}