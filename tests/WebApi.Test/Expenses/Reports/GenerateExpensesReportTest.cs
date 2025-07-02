using FluentAssertions;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Reports;

public class GenerateExpensesReportTest(CustomWebApplicationFactory factory) : CashFlowClassFixture(factory)
{
    private readonly string ENDPOINT = "report";

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{ENDPOINT}/excel?month={_expense_Admin.GetDate():yyyy-MM}", token: _user_Admin.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{ENDPOINT}/pdf?month={_expense_Admin.GetDate():yyyy-MM}", token: _user_Admin.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        result.Content.Headers.ContentType.Should().NotBeNull();
        result.Content.Headers.ContentType.MediaType.Should().Be(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{ENDPOINT}/excel?month={_expense_Admin.GetDate():yyyy-MM}", token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{ENDPOINT}/pdf?month={_expense_Admin.GetDate():yyyy-MM}", token: _user_Team_Member.GetToken());

        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
