using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extensions;

public static class ConfigurationExtension
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
}
