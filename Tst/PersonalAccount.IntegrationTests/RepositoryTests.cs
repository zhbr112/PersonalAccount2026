using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using PersonalAccount.Console.Logics;
using PersonalAccount.Console.Models;

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
    // Настройки текущие
    private ApplicationOptions _options;

    public RepositoryTests()
    {
        var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        _options = configuration.Get<ApplicationOptions>()
                        ?? throw new InvalidOperationException("Unabled loading appsettings.json!");
    }

    /// <summary>
    /// Проверить работу репозитория для выборки данных.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetRows_JournalRepository_Any()
    {
        // Подготовка
        using var connect = new SqlConnection (_options.ConnectionString);
        var repo = new JournalRepository();

        // Действие
        var result = await repo.GetRows(connect, new Domain.Models.LoadingSettings() { BatchSize = 100});

        // Проверки
        Assert.That(result is not null);
        Assert.That(result!.Any());
        Assert.That(result!.Count() == 100);
    }
}
