namespace CashFlow.Domain.Repositories;

public interface IUnitOfWork
{
    public void Commit();
}
