using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UserRepository(CashFlowDbContext context) : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly CashFlowDbContext _context = context;

    public async Task Add(User user) => await _context.Users.AddAsync(user);

    public async Task Delete(User user)
    {
        var userToDelete = await _context.Users.FindAsync(user.Id);

        _context.Users.Remove(userToDelete!);
    }

    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Email.Equals(email));

    public Task<User?> GetByEmail(string email) =>
        _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email));

    public async Task<User> GetById(long id) =>
       await _context.Users
            .FirstAsync(u => u.Id == id);

    public void Update(User user) => _context.Users.Update(user);
}
