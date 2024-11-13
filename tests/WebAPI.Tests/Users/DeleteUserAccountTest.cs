using System.Net;
using FluentAssertions;

namespace WebAPI.Test.Users;

public class DeleteUserAccountTest : CashFlowClassFixture
{
    private const string USER_URI = "api/User";
    
    private readonly string _token;

    public DeleteUserAccountTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DeleteAsync(USER_URI, _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}