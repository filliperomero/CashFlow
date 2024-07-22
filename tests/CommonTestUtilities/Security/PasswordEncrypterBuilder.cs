using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Security;

public class PasswordEncrypterBuilder
{
    public static IPasswordEncrypter Build()
    {
        var mock = new Mock<IPasswordEncrypter>();

        mock
            .Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>()))
            .Returns("!Password-encrypted-123");
        
        return mock.Object;
    }
}