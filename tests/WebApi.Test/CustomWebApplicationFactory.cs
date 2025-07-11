using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens.Generator;
using CashFlow.Infrastructure.DataAccess;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager Expense_Team_Member { get; private set; } = default!;
    public ExpenseIdentityManager Expense_Admin { get; private set; } = default!;

    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;

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
                var accessTokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(context, passwordEncripter, accessTokenGenerator);
            });
    }

    private void StartDatabase(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var userTeamMember = AddTeamMemberUser(context, passwordEncripter, accessTokenGenerator);
        var expenseTeamMember = AddExpenses(context, userTeamMember, expenseId: 1, tagId: 1);
        Expense_Team_Member = new ExpenseIdentityManager(expenseTeamMember);

        var userAdmin = AddAdminUser(context, passwordEncripter, accessTokenGenerator);
        var expenseAdmin = AddExpenses(context, userAdmin, expenseId: 2, tagId: 2);
        Expense_Admin = new ExpenseIdentityManager(expenseAdmin);

        context.SaveChanges();
    }

    private Expense AddExpenses(CashFlowDbContext context, User user, long expenseId, long tagId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        foreach (var tag in expense.Tags)
        {
            tag.Id = tagId;
            tag.ExpenseId = expenseId;
        }

        context.Expenses.Add(expense);

        return expense;
    }

    private User AddTeamMemberUser(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddAdminUser(CashFlowDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator accessTokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;
        
        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = accessTokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }
}
