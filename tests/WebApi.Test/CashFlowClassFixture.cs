using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CashFlowClassFixture(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient = factory.CreateClient();

    protected readonly UserIdentityManager _user_Team_Member = factory.User_Team_Member;
    protected readonly UserIdentityManager _user_Admin = factory.User_Admin;

    protected readonly ExpenseIdentityManager _expense_Team_Member = factory.Expense_Team_Member;
    protected readonly ExpenseIdentityManager _expense_Admin = factory.Expense_Admin;

    protected async Task<HttpResponseMessage> DoPost(string requestUri, object request, string token = "", string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string requestUri, string token, string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.GetAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> DoDelete(string requestUri, string token, string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.DeleteAsync(requestUri);
    }

    protected async Task<HttpResponseMessage> DoPut(string requestUri, object request, string token, string culture = "en")
    {
        AuthorizeRequest(token);
        ChangeRequestCulture(culture);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    private void AuthorizeRequest(string token)
    {
        if (!string.IsNullOrEmpty(token))
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private void ChangeRequestCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));
    }
}
