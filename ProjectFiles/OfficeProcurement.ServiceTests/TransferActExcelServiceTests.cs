using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentAssertions;
using OfficeProcurement.Entities;
using OfficeProcurement.Entities.Enums;
using OfficeProcurement.Repositories;
using OfficeProcurement.Services.Contracts.Interface;
using OfficeProcurement.Services.Contracts.Models;
using OfficeProcurement.Services.Implementations;
using Xunit;

namespace OfficeProcurement.ServiceTests;

/// <summary>
/// Тесты для <see cref="TransferActExcelService"/>
/// </summary>
public class TransferActExcelServiceTests : BaseServiceTests
{
    private readonly ITransferActExcelService excelService;

    /// <summary>Ы
    /// Инициализирует тесты сервиса экспорта в Excel
    /// </summary>
    public TransferActExcelServiceTests()
    {
        var transferActRepository = new TransferActRepository(WriterContext, Context);
        var transferActItemRepository = new TransferActItemRepository(WriterContext, Context);
        excelService = new TransferActExcelService(transferActRepository, transferActItemRepository);
    }

    /// <summary>
    /// Проверяет что генерация Excel возвращает непустой файл с основными данными акта
    /// </summary>
    [Fact]
    public async Task ExportTransferActAsync_ShouldGenerateValidExcel()
    {
        // Arrange
        var actModel = await SeedTransferActAsync(
            number: "А-100",
            quantity: 2m,
            unitPrice: 1500m);

        // Act
        var bytes = await excelService.ExportTransferActAsync(actModel, CancellationToken.None);

        // Assert
        bytes.Should().NotBeEmpty();
        var cells = ReadAllCellValues(bytes);
        cells.Should().Contain("АКТ №А-100");
        cells.Should().Contain("г. Москва");
        cells.Should().Contain("Ноутбук");
    }

    /// <summary>
    /// Проверяет что дробное количество товара (2.5) не обрезается до целого
    /// </summary>
    [Fact]
    public async Task ExportTransferActAsync_WithFractionalQuantity_ShouldPreserveDecimals()
    {
        // Arrange
        var actModel = await SeedTransferActAsync(
            number: "А-101",
            quantity: 2.5m,
            unitPrice: 100m);

        // Act
        var bytes = await excelService.ExportTransferActAsync(actModel, CancellationToken.None);

        // Assert
        var cells = ReadAllCellValues(bytes);
        cells.Should().Contain("2.5");
    }

    /// <summary>
    /// Проверяет корректность расчёта НДС (20%) и копеек в строке ИТОГО
    /// </summary>
    [Fact]
    public async Task ExportTransferActAsync_WithVat_ShouldCalculateCorrectly()
    {
        // Arrange
        var actModel = await SeedTransferActAsync(
            number: "А-102",
            quantity: 2m,
            unitPrice: 100.25m);

        // Act
        var bytes = await excelService.ExportTransferActAsync(actModel, CancellationToken.None);

        // Assert
        var cells = ReadAllCellValues(bytes);
        cells.Should().Contain("200.50");
        cells.Should().Contain("40.10");
        cells.Should().Contain(c => c.Contains("ИТОГО: 200 рублей 50 копеек"));
    }

    /// <summary>
    /// Проверяет, что пустое имя контактного лица не вызывает ошибку при экспорте
    /// </summary>
    [Fact]
    public async Task ExportTransferActAsync_WithEmptyContactName_ShouldNotThrow()
    {
        // Arrange
        var actModel = await SeedTransferActAsync(
            number: "А-103",
            quantity: 1m,
            unitPrice: 500m,
            sellerContactName: "",
            buyerContactName: "");

        // Act
        var bytes = await excelService.ExportTransferActAsync(actModel, CancellationToken.None);

        // Assert
        bytes.Should().NotBeEmpty();
    }

    private async Task<TransferActModel> SeedTransferActAsync(
    string number,
    decimal quantity,
    decimal unitPrice,
    string? sellerContactName = "Иван",
    string? buyerContactName = "Пётр")
    {
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Inn = "1234567890",
            CompanyName = "ООО Поставщик",
            CeoBasis = "Устава",
            ContactLastName = "Иванов",
            ContactName = sellerContactName,
            ContactPatronymic = "Иванович"
        };
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Inn = "0987654321",
            CompanyName = "ООО Покупатель",
            ContactLastName = "Петров",
            ContactName = buyerContactName,
            ContactPatronymic = "Петрович"
        };
        var contract = new Contract
        {
            Id = Guid.NewGuid(),
            Number = "Д-001",
            SignedAt = DateTimeOffset.Now,
            Type = ContractType.Supply,
            SupplierId = supplier.Id,
            ClientId = client.Id
        };
        var transferAct = new TransferAct
        {
            Id = Guid.NewGuid(),
            Number = number,
            Date = DateOnly.FromDateTime(DateTime.Now),
            City = "Москва",
            ContractId = contract.Id
        };
        var item = new TransferActItem
        {
            Id = Guid.NewGuid(),
            Name = "Ноутбук",
            Unit = "шт",
            Quantity = quantity,
            UnitPrice = unitPrice,
            TransferActId = transferAct.Id
        };

        Add(supplier);
        Add(client);
        Add(contract);
        Add(transferAct);
        Add(item);
        await UnitOfWork.SaveChangesAsync(CancellationToken.None);

        return new TransferActModel
        {
            Id = transferAct.Id,
            Number = transferAct.Number,
            Date = transferAct.Date,
            City = transferAct.City,
            ContractId = contract.Id
        };
    }

    private static List<string> ReadAllCellValues(byte[] excelBytes)
    {
        using var stream = new MemoryStream(excelBytes);
        using var document = SpreadsheetDocument.Open(stream, false);
        var sheetData = document.WorkbookPart!.WorksheetParts.First().Worksheet.GetFirstChild<SheetData>()!;
        return sheetData.Descendants<Cell>()
            .Where(c => c.CellValue != null)
            .Select(c => c.CellValue!.Text)
            .ToList();
    }

}
