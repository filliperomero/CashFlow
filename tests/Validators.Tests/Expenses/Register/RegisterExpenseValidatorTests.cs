using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.Expenses.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("             ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_Future()
    {
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        request.Date = DateTime.UtcNow.AddDays(2);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPENSE_CANNOT_HAVE_FUTURE_DATE));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        request.PaymentType = (PaymentType)999;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Error_Amount_Invalid(decimal amount)
    {
        var validator = new ExpenseValidator();
        var request = RequestExpenseJsonBuilder.Build();

        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
}
