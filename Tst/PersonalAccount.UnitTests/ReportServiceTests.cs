using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Api.Extensions;
using PersonalAccount.Common.Core;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Data.Extensions;

namespace PersonalAccount.UnitTests;

/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/

/// <summary>
/// Набор модульных тестов для проверки работы отчетов.
/// </summary>
public class ReportServiceTests
{
    // Работа с контейнером
    private IServiceProvider _provider;

    public ReportServiceTests()
    {
       var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        var services = new ServiceCollection()
                     .RegistryPersonalAccountData( configuration )
                     .RegistryPersonalAccountApi( configuration )
                     .RegistryPersonalAccountConsole( configuration);

        _provider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Проверить работу метода Create класс RevenueReportService
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Create_RevenueReportService_Check()
    {
        // Подготовка
        const double typicalResult = 820.2;
        var service = _provider.GetRequiredService<IRevenueReportService>();
        var creator = new ReportDataCreator();
        creator.BuildTypicalPaymentScenario(); 


        // Действие
        var result = await service.CreateAsync( creator.Transactions, CancellationToken.None );

        // Проверка
        Assert.That(result.Any());
        Assert.That(result.First().BankAmount == typicalResult);
        Assert.That(result.First().CashAmount == typicalResult);
    }

    /// <summary>
    /// Проверить работу метода Create класс SalesReportService
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Create_SalesReportService_Check()
    {
        // Подготовка
        const double typicalResult = 820.2;
        var service = _provider.GetRequiredService<ISalesReportService>();
        var creator = new ReportDataCreator();
        creator.BuildTypicalPaymentScenario();

        // Действие
        var result = await service.CreateAsync( creator.Transactions, CancellationToken.None );

        // Проверка
        Assert.That(result.Any());
        Assert.That(result.Sum( x => x.Amount) == typicalResult * 2);
    }

    /// <summary>
    /// Проверить работу метода Create класс WorkScheduleReportService
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Create_WorkScheduleReportService_Check()
    {
        // Подготовка
        var service = _provider.GetRequiredService<IWorkScheduleReportService>();
        var creator = new ReportDataCreator();
        creator.BuildTypicalWorkSheduleScenario();

        // Действие
        var result = await service.CreateAsync( creator.Transactions, CancellationToken.None );

        // Проверка
        Assert.That(result.Any());
        Assert.That(result.Count() == 2);
    }
}
