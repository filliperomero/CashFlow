using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CommonTestUtilities.Security;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Builder(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        
        return new Faker<User>()
            .RuleFor(user => user.Id, _ => 1)
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (_, user) => passwordEncrypter.Encrypt(user.Password))
            .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
            .RuleFor(user => user.Role, _ => role);
    }
}