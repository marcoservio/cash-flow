﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesUpdateOnlyRepository
{
    void Update(Expense expense);
    Task<Expense?> GetById(Entities.User user, long id);
}
