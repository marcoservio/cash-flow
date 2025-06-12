using CashFlow.Domain.Entities;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository readOnlyRepository) : IGenerateExpensesReportExcelUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private readonly IExpensesReadOnlyRepository _readOnlyRepository = readOnlyRepository;

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _readOnlyRepository.FilterByMonth(month);

        if (expenses.Count == 0)
            return [];

        using var workbook = new XLWorkbook
        {
            Author = "CashFlow"
        };

        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheet = workbook.Worksheets.Add(month.ToString("Y"));

        InserHeader(worksheet);

        var row = 2;
        foreach (var expense in expenses)
        {
            WriteExpenseRow(worksheet, row, expense);

            row++;
        }

        worksheet.Columns().AdjustToContents();

        var file = new MemoryStream();

        workbook.SaveAs(file);

        return file.ToArray();
    }

    private static void WriteExpenseRow(IXLWorksheet worksheet, int row, Expense expense)
    {
        worksheet.Cell($"A{row}").Value = expense.Title;
        worksheet.Cell($"B{row}").Value = expense.Date;
        worksheet.Cell($"C{row}").Value = expense.PaymentType.PaymentTypeToString();

        worksheet.Cell($"D{row}").Value = expense.Amount;
        worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{CURRENCY_SYMBOL} #,##0.00";

        worksheet.Cell($"E{row}").Value = expense.Description;
    }

    private static void InserHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;

        worksheet.Cell("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}
