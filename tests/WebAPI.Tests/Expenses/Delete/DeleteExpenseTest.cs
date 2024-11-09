using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using FluentAssertions;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Expenses.Delete;

public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string EXPENSE_URI = "api/Expenses";
    
    private readonly string _token;
    private readonly long _expenseId;

    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DeleteAsync(requestUri: $"{EXPENSE_URI}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        
        result = await GetAsync(requestUri: $"{EXPENSE_URI}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string cultureInfo)
    {
        var result = await DeleteAsync(requestUri: $"{EXPENSE_URI}/1000", token: _token, culture: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}