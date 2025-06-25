using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token;

public class HttpContentTokenValue(IHttpContextAccessor httpContextAccessor) : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string TokenOnRequest()
    {
        var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
