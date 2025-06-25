using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly string ENDPOINT = "user";
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await _httpClient.PostAsJsonAsync(ENDPOINT, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(culture));

        var result = await _httpClient.PostAsJsonAsync(ENDPOINT, request);

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
