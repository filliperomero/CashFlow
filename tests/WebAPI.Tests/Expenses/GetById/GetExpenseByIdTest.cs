using System.Globalization;
using System.Net;
using System.Text.Json;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using FluentAssertions;
using WebAPI.Test.InlineData;

namespace WebAPI.Test.Expenses.GetById;

public class GetExpenseByIdTest : CashFlowClassFixture
{
    private const string EXPENSE_URI = "api/Expenses";
    
    private readonly string _token;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
        _expenseId = webApplicationFactory.GetExpenseId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await GetAsync(requestUri: $"{EXPENSE_URI}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var body = await result.Content.ReadAsStreamAsync();
        
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_expenseId);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
        
        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();
    }
    
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string cultureInfo)
    {
        var result = await GetAsync(requestUri: $"{EXPENSE_URI}/1000", token: _token, culture: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));

        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}