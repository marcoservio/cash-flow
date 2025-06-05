using CashFlow.Application.UseCases.Expenses.Reports.Excel;

using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;

namespace CashFlow.Api.Controllers;

public class ReportController : CashFlowControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateExpensesReportExcelUseCase useCase,
        [FromHeader] DateOnly month)
    {
        byte[] file = await useCase.Execute(month);

        if (file.Length == 0)
            return NoContent();

        return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
    }
}
