using CashFlow.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }

    public static DatabaseTypes DatabaseType(this IConfiguration configuration)
    {
        var databaseType = configuration.GetConnectionString("DatabaseType");

        return (DatabaseTypes)Enum.Parse(typeof(DatabaseTypes), databaseType!);
    }

    public static string ConnectionString(this IConfiguration configuration)
    {
        var databaseType = configuration.DatabaseType();

        if (databaseType == DatabaseTypes.MySQL)
        {
            var connectionString = configuration.GetConnectionString("ConnectionMySQLServer")!;

            return string.IsNullOrWhiteSpace(connectionString)
               ? Environment.GetEnvironmentVariable("CONNECTION_MYSQL_SERVER") ?? string.Empty
               : connectionString ?? string.Empty!;
        }
        else
            return configuration.GetConnectionString("ConnectionSQLServer")!;
    }
}
