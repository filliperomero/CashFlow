﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

[Obsolete]
public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
}
