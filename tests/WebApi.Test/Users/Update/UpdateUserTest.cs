using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update;

public class UpdateUserTest(CustomWebApplicationFactory factory) : CashFlowClassFixture(factory)
{
    private readonly string ENDPOINT = "user";

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: ENDPOINT, request: request, token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: ENDPOINT, token: _user_Team_Member.GetToken());

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("email").GetString().Should().Be(request.Email);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(requestUri: ENDPOINT, request: request, token: _user_Team_Member.GetToken(), culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        response.RootElement.GetProperty("errors")
            .EnumerateArray()
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
