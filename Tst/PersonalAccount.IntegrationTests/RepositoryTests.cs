using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Common.Models;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Console.Logics;
using PersonalAccount.Data.Extensions;

namespace PersonalAccount.IntegrationTests;

/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


/// <summary>
/// Набор интеграционных тестов для проверки работы различных репозиториев.
/// </summary>
public class RepositoryTests
{
     // Работа с контейнером
    private IServiceProvider _provider;

    public RepositoryTests()
    {
       var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        var services = new ServiceCollection()
                     .RegistryPersonalAccountData( configuration )
                     .RegistryPersonalAccountConsole( configuration);

        _provider = services.BuildServiceProvider();
    }

    /// <summary>
    /// Простой тест для замера производительности.
    /// </summary>
    /// <param name="rows"></param>
    /// <returns></returns>
    [Test]
    [TestCase(100)]
    [TestCase(1000)]
    [TestCase(10000)]
    public async Task GetRows_JournalRepository_Fetch(int rows)
    {
        // Подготовка
        var options = _provider.GetRequiredService<ConsoleOptions>();
        using var connect = new SqlConnection(options.ConnectionString);
        var repo = new JournalRepository();

        // Действие
        var result = await repo.GetRows(connect, new Domain.Models.LoadingSettingsModel() { BatchSize = rows });

        // Проверки
        Assert.That(result is not null);
        Assert.That(result!.Any());
        Assert.That(result!.Count() == rows);
    }
}
