namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public interface IGenerateExpensesReportPdfUseCase
{
    public Task<byte[]> Execute(DateOnly date);
}
