using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GeAll;

public class GetAllExpenseTest(CustomWebApplicationFactory factory) : CashFlowClassFixture(factory)
{
    private readonly string ENDPOINT = "expenses";

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: ENDPOINT, token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("expenses").EnumerateArray().Should().NotBeNullOrEmpty();
    }
}
