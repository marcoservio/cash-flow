using CashFlow.Domain.Security.Cryptography;
using BC = BCrypt.Net.BCrypt;

namespace CashFlow.Infrastructure.Security.Cryptography;

public class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password) => BC.HashPassword(password);

    public bool Verify(string password, string hashedPassword) => BC.Verify(password, hashedPassword);
}
