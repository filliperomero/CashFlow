﻿using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private readonly IExpensesWriteOnlyRepository _expensesRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository, IUnitOfWork unitOfWork, ILoggedUser loggedUser, IExpensesReadOnlyRepository expensesReadOnlyRepository)
    {
        _expensesRepository = repository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var expense = await _expensesReadOnlyRepository.GetById(loggedUser, id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        await _expensesRepository.Delete(id);
        await _unitOfWork.Commit();
    }
}
