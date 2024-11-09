using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Expenses.Update;

public class UpdateExpenseTest : CashFlowClassFixture
{
    private const string EXPENSE_URI = "api/Expenses";
    
    private readonly string _token;
    private readonly long _expenseId;

    public UpdateExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();
        
        var result = await PutAsync(requestUri: $"{EXPENSE_URI}/{_expenseId}", request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title(string cultureInfo)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await PutAsync(requestUri: $"{EXPENSE_URI}/{_expenseId}", request: request, token: _token, culture: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var body = await result.Content.ReadAsStreamAsync();
        
        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string cultureInfo)
    {
        var request = RequestExpenseJsonBuilder.Build();
        
        var result = await PutAsync(requestUri: $"{EXPENSE_URI}/1000", request: request, token: _token, culture: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}