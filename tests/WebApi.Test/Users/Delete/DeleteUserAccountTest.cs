using CashFlow.Communication.Requests;
using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete;

public class DeleteUserAccountTest(CustomWebApplicationFactory factory) : CashFlowClassFixture(factory)
{
    private readonly string ENDPOINT = "user";

    [Fact]
    public async Task Success_Team_Member()
    {
        var result = await DoDelete(requestUri: ENDPOINT, token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson()
        {
            Email = _user_Team_Member.GetEmail(),
            Password = _user_Team_Member.GetPassword()
        };

        result = await DoPost(requestUri: "login", request: loginRequest);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Success_Admin()
    {
        var result = await DoDelete(requestUri: ENDPOINT, token: _user_Admin.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson()
        {
            Email = _user_Admin.GetEmail(),
            Password = _user_Admin.GetPassword()
        };

        result = await DoPost(requestUri: "login", request: loginRequest);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
