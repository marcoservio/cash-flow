using FluentAssertions;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Profile;

public class GetUserProfileTest(CustomWebApplicationFactory factory) : CashFlowClassFixture(factory)
{
    private readonly string ENDPOINT = "user";

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: ENDPOINT, token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(_user_Team_Member.GetName());
        response.RootElement.GetProperty("email").GetString().Should().Be(_user_Team_Member.GetEmail());
    }
}
