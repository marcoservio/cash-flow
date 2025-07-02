using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Update;

public class UpdateExpenseTest(CustomWebApplicationFactory factory) : CashFlowClassFixture(factory)
{
    private readonly string ENDPOINT = "expenses";

    [Fact]
    public async Task Success()
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{ENDPOINT}/{_expense_Team_Member.GetExpenseId()}", request: request, token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{ENDPOINT}/{_expense_Team_Member.GetExpenseId()}", token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_expense_Team_Member.GetExpenseId());
        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
        response.RootElement.GetProperty("description").GetString().Should().Be(request.Description);
        response.RootElement.GetProperty("amount").GetDecimal().Should().Be(request.Amount);
        response.RootElement.GetProperty("date").GetDateTime().Should().Be(request.Date);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Required(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPut(requestUri: $"{ENDPOINT}/{_expense_Team_Member.GetExpenseId()}", request: request, token: _user_Team_Member.GetToken(), culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        response.RootElement.GetProperty("errors")
            .EnumerateArray()
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.GetString()!.Equals(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string culture)
    {
        var request = RequestExpenseJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{ENDPOINT}/{1000}", request: request, token: _user_Team_Member.GetToken(), culture: culture);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(culture));

        response.RootElement.GetProperty("errors")
            .EnumerateArray()
            .Should()
            .ContainSingle()
            .And
            .Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}