using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeProcurement.Dal.Contracts.Repositories;
using OfficeProcurement.Entities;
using OfficeProcurement.Repositories.Contracts;
using OfficeProcurement.Services.Contracts.Interface;

namespace OfficeProcurement.Services.Implementations;

/// <summary>
/// Сервис экспорта акта передачи в Excel
/// </summary>
public class TransferActExcelService : ITransferActExcelService
{
    private readonly ITransferActRepository transferActRepository;
    private readonly ITransferActItemRepository transferActItemRepository;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="TransferActExcelService"/>
    /// </summary>
    public TransferActExcelService(
        ITransferActRepository transferActRepository,
        ITransferActItemRepository transferActItemRepository)
    {
        this.transferActRepository = transferActRepository;
        this.transferActItemRepository = transferActItemRepository;
    }

    /// <inheritdoc/>
    public async Task<byte[]> ExportTransferActAsync(Guid transferActId, CancellationToken cancellationToken)
    {
        var transferAct = await transferActRepository.GetTransferActWithContractByIdAsync(transferActId, cancellationToken);
        if (transferAct?.Contract == null)
        {
            throw new InvalidOperationException($"Акт передачи или договор не найден");
        }

        var contract = transferAct.Contract;
        var seller = contract.Supplier;
        var buyer = contract.Client;

        var items = await transferActItemRepository.GetTransferActItemsByTransferActIdAsync(transferActId, cancellationToken);
        var itemList = items?.ToList() ?? new List<TransferActItem>();

        using var stream = new MemoryStream();
        using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = document.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            var worksheet = new Worksheet();
            worksheetPart.Worksheet = worksheet;

            var sheets = workbookPart.Workbook.AppendChild(new Sheets());
            var sheet = new Sheet
            {
                Id = workbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "Акт приема-передачи"
            };
            sheets.Append(sheet);

            var sheetData = worksheet.AppendChild(new SheetData());

            AppendRow(sheetData, [$"АКТ №{transferAct.Number}"], true, true);
            AppendRow(sheetData, ["приема-передачи имущества"], true, true);
            AppendRow(sheetData, [$"к договору купли-продажи №{contract.Number}"], true, true);
            AppendRow(sheetData, Array.Empty<string>());

            AppendRow(sheetData, [$"г. {transferAct.City}", "", "", "", "", $"«{transferAct.Date.Day}» {GetMonthName(transferAct.Date.Month)} {transferAct.Date.Year} года"]);
            AppendRow(sheetData, Array.Empty<string>());

            AppendRow(sheetData, [ $"{seller.CompanyName}, именуемое в дальнейшем «Продавец», в лице директора, действующего на основании {seller.CeoBasis}, " +
                $"с одной стороны, и {buyer.CompanyName}, именуемое в дальнейшем «Покупатель», с другой стороны, подписали настоящий Акт о нижеследующем:" ]);
            AppendRow(sheetData, Array.Empty<string>());

            AppendRow(sheetData, ["1. Продавец передал, а покупатель принял в собственность следующее имущество:"]);
            AppendRow(sheetData, Array.Empty<string>());

            AppendRow(sheetData, ["№ п/п", "Наименование имущества", "Ед. изм.", "Кол-во", "Цена за ед. (руб.)", "НДС 20% (руб.)", "Общая стоимость (руб.)"], true, true);

            decimal totalSum = 0;
            decimal totalVat = 0;
            for (int i = 0; i < itemList.Count; i++)
            {
                var item = itemList[i];
                var sum = item.Quantity * item.UnitPrice;
                var vat = sum * 0.20m;
                totalSum += sum;
                totalVat += vat;

                AppendRow(sheetData,
                [
                (i + 1).ToString(),
                item.Name,
                item.Unit,
                ((int)item.Quantity).ToString(),
                item.UnitPrice.ToString("F2"),
                vat.ToString("F2"),
                sum.ToString("F2")
            ]);
            }

            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, [$"ИТОГО: {totalSum:F2} рублей 00 копеек, в т.ч. НДС {totalVat:F2} рублей."], true, true);
            AppendRow(sheetData, Array.Empty<string>());

            AppendRow(sheetData, ["2. Стороны совместно при приеме-передаче имущества произвели его осмотр и пришли к соглашению, " +
                "что передаваемое имущество находится в исправном состоянии и соответствует требованиям и условиям Договора."]);
            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, ["3. Покупатель каких-либо претензий к Продавцу не имеет."]);
            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, ["4. Настоящий Акт составлен и подписан в двух экземплярах, имеющих одинаковую юридическую силу, по одному для каждой из Сторон."]);
            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, Array.Empty<string>());

            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, ["Продавец:", "", "", "", "", "Покупатель:"], true, true);
            AppendRow(sheetData, Array.Empty<string>());

            var sellerDirector = $"{seller.ContactLastName} {seller.ContactName[0]}.{(seller.ContactPatronymic?.Length > 0 ? seller.ContactPatronymic[0] + "." : "")}";
            var buyerDirector = $"{buyer.ContactLastName} {buyer.ContactName[0]}.{(buyer.ContactPatronymic?.Length > 0 ? buyer.ContactPatronymic[0] + "." : "")}";

            AppendRow(sheetData, [$"Директор {sellerDirector}", "", "", "", "", $"Директор {buyerDirector}"]);
            AppendRow(sheetData, Array.Empty<string>());
            AppendRow(sheetData, ["_______________", "", "", "", "", "_______________"]);
        }

        return stream.ToArray();
    }

    private static string GetMonthName(int month)
    {
        var months = new[] { "", "января", "февраля", "марта", "апреля", "мая", "июня",
                         "июля", "августа", "сентября", "октября", "ноября", "декабря" };
        return months[month];
    }

    private static void AppendRow(SheetData sheetData, string[] cells, bool isBold = false, bool isCentered = false)
    {
        var row = new Row();
        foreach (var cellValue in cells)
        {
            var cell = new Cell
            {
                CellValue = new CellValue(cellValue),
                DataType = CellValues.String
            };
            row.Append(cell);
        }
        sheetData.Append(row);
    }
}
