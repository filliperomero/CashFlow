using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;
public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage("The title is required.");
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage("The amount should be greater than zero.");
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Expenses cannot have a future date.");
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage("PaymentType is not valid.");
    }
}
