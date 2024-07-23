using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(r => r.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(r => r.Email, faker => faker.Internet.Email());
    }
}