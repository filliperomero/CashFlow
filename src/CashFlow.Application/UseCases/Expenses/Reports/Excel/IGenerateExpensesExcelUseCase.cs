namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public interface IGenerateExpensesExcelUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}
