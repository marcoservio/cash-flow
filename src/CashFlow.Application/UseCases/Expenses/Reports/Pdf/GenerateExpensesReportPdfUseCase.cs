using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "R$";
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;

    private readonly IExpensesReadOnlyRepository _readOnlyRepository;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository readOnlyRepository)
    {
        _readOnlyRepository = readOnlyRepository;

        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expenses = await _readOnlyRepository.FilterByMonth(month);
        if (expenses.Count == 0)
            return [];

        var document = CreateDocument(month);
        var page = CreatePage(document);

        CreateHeaderWithProfilePhotoAndName(page);

        CreateTotalSpentSection(page, month, expenses.Sum(e => e.Amount));

        foreach (var expense in expenses)
        {
            var table = CreateExpenseTable(page);

            AddExpenseDetailsToTable(expense, table);
        }

        return RenderDocument(document);
    }

    private static void AddExpenseDetailsToTable(Domain.Entities.Expense expense, Table table)
    {
        var row = AddNewBlankRow(table);

        AddExpenseTitle(row.Cells[0], expense.Title);

        AddHeaderForAmount(row.Cells[3]);

        row = AddNewBlankRow(table);

        row.Cells[0].AddParagraph(expense.Date.ToString("D"));
        SetStyleBaseForExpenseInformation(row.Cells[0]);
        row.Cells[0].Format.LeftIndent = 20;

        row.Cells[1].AddParagraph(expense.Date.ToString("t"));
        SetStyleBaseForExpenseInformation(row.Cells[1]);

        row.Cells[2].AddParagraph(expense.PaymentType.PaymentTypeToString());
        SetStyleBaseForExpenseInformation(row.Cells[2]);

        AddAmountForExpense(row.Cells[3], expense.Amount);

        AddDescriptionForExpense(expense.Description, table, row);

        AddWhiteSpace(table);
    }

    private static Row AddNewBlankRow(Table table)
    {
        Row row = table.AddRow();
        row.Height = HEIGHT_ROW_EXPENSE_TABLE;
        return row;
    }

    private static void AddDescriptionForExpense(string? description, Table table, Row row)
    {
        if (!string.IsNullOrWhiteSpace(description))
        {
            var descriptionRow = table.AddRow();
            descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

            descriptionRow.Cells[0].AddParagraph(description);
            descriptionRow.Cells[0].Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
            descriptionRow.Cells[0].Shading.Color = ColorsHelper.GREEN_LIGHT;
            descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
            descriptionRow.Cells[0].MergeRight = 2;
            descriptionRow.Cells[0].Format.LeftIndent = 20;

            row.Cells[3].MergeDown = 1;
        }
    }

    private static void AddExpenseTitle(Cell cell, string expenseTitle)
    {
        cell.AddParagraph(expenseTitle);
        cell.Format.Font = new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private static void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.RightIndent = 10;
    }

    private static Document CreateDocument(DateOnly month)
    {
        var document = new Document
        {
            Info = new DocumentInfo
            {
                Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} - {month:Y}",
                Author = "CashFlow Application"
            }
        };

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.DEFAULT_FONT;

        return document;
    }

    private static Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private static void CreateHeaderWithProfilePhotoAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "logo.png");

        row.Cells[0].AddImage(pathFile);

        row.Cells[1].AddParagraph("Hey, Marco Sérvio");
        row.Cells[1].Format.Font = new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 16 };
        row.Cells[1].VerticalAlignment = VerticalAlignment.Center;
    }

    private static void CreateTotalSpentSection(Section page, DateOnly month, decimal totalExpenses)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = 40;
        paragraph.Format.SpaceAfter = 40;

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));
        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RELEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak();

        paragraph.AddFormattedText($"{CURRENCY_SYMBOL} {totalExpenses:N2}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });

        paragraph.AddLineBreak();
    }

    private static Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        var column = table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private static void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"{CURRENCY_SYMBOL} {amount}");
        cell.Format.Font = new Font { Name = FontHelper.WORKSANS_REGULAR, Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private static byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        using var stream = new MemoryStream();

        renderer.PdfDocument.Save(stream);

        return stream.ToArray();
    }
}
