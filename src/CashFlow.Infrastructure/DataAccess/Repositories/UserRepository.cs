using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UserRepository(CashFlowDbContext context) : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly CashFlowDbContext _context = context;

    public async Task Add(User user) => await _context.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.Equals(email));

    public Task<User?> GetByEmail(string email) =>
        _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
}
