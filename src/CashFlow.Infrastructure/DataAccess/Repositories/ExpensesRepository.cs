using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(CashFlowDbContext context) : IExpensesReadOnlyRepository, IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context = context;

    public async Task Add(Expense expense) => await _context.Expenses.AddAsync(expense);

    public async Task Delete(long id)
    {
        var result = await _context.Expenses.FindAsync(id);

        _context.Expenses.Remove(result!);
    }

    public async Task<List<Expense>> GetAll(User user) => await _context.Expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id) =>
             await _context.Expenses
             .AsNoTracking()
             .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user, long id) =>
            await _context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);

    public void Update(Expense expense) => _context.Expenses.Update(expense);

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        return await _context
            .Expenses
            .AsNoTracking()
            .Where(e =>e.UserId == user.Id && e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync();
    }
}
