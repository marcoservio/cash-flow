using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class UserIdentityManager(User user, string password, string token)
{
    private User _user = user;
    private string _password = password;
    private string _token = token;

    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;
    public string GetToken() => _token;
}
