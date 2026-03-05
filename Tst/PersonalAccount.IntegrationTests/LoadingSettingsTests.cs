using System;
using NUnit.Framework;
using PersonalAccount.Data.Logics;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.IntegrationTests;


/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


public class LoadingSettingsTests
{
    /// <summary>
    /// Проверить выборку настроек из репозитория.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task  Load_LoadingSettingsRepository_NotThrow()
    {
        // Подготова
        var repo = new LoadingSettingsRepository();
        var company = new Company()
        {
            Id = new Guid( "14e54725-0efc-42b8-a27d-a84f9a7257c5")
        };

        // Проверки и действие
        Assert.DoesNotThrowAsync( async() =>
        {
            var result = await repo.Load(company, CancellationToken.None);
            Assert.That(result is not null);
        });
    }
}
