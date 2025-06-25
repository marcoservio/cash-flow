using CashFlow.Domain.Security.Cryptography;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private CashFlow.Domain.Entities.User _user = default!;
    private string _password = string.Empty;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();

                StartDatabase(context, passwordEncripter);
            });
    }

    public string GetName() => _user.Name;
    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;

    private void StartDatabase(CashFlowDbContext context, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncripter.Encrypt(_user.Password);

        context.Users.Add(_user);

        context.SaveChanges();
    }
}
