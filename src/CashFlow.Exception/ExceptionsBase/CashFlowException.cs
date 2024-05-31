namespace CashFlow.Exception.ExceptionsBase;

public abstract class CashFlowException : System.Exception
{
    protected CashFlowException(string? message) : base(message) { }

    public abstract int StatusCode {  get; }

    public abstract List<string> GetErrors();
}
