namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public interface IGenerateExpensesReportExcelUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}
