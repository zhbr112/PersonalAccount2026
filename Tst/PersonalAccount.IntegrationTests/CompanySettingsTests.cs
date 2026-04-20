using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PersonalAccount.Common.Core;
using PersonalAccount.Data.Extensions;
using PersonalAccount.Data.Logics;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.IntegrationTests;


/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


public class CompanySettingsTests
{

   // Работа с контейнером
    private IServiceProvider _provider;

    public CompanySettingsTests()
    {
       var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

        var configuration = builder.Build();
        var services = new ServiceCollection()
                     .RegistryPersonalAccountData( configuration );

        _provider = services.BuildServiceProvider();
    }


    /// <summary>
    /// Прверить работу метода Load класса <see cref="CompanySettingsRepository"/>
    /// </summary>
    /// <returns></returns>
    [Test]
    [TestCase("14e54725-0efc-42b8-a27d-a84f9a7257c5")]
    [Order(2)]
    public async Task  Load_CompanySettingsRepository_NotThrow(string companyId)
    {
        // Подготова
        var repo = _provider.GetRequiredService<ICompanySettingsRepository>();
        var context = _provider.GetRequiredService<PersonalAccountContext>();
        var company = new CompanyModel()
        {
            Id = new Guid( companyId )
        };
        var branchId = context.Branches.Where(x => x.CompanyId == company.Id).Select(x => x.Id).First();

        // Проверки и действие
        Assert.DoesNotThrowAsync( async() =>
        {
            var result = await repo.LoadAsync(branchId, CancellationToken.None);
            Assert.That(result is not null);
        });
    }

    /// <summary>
    /// Проверить работу метода Save класса <see cref="CompanySettingsRepository"/>
    /// </summary>
    /// <returns></returns>
    [Test]
    [TestCase("14e54725-0efc-42b8-a27d-a84f9a7257c5")]
    [Order(1)]
    public async Task Save_CompanySettingsRepository_NotThrow(string companyId)
    {
        // Подготовка
        var repo = _provider.GetRequiredService<ICompanySettingsRepository>();
        var context = _provider.GetRequiredService<PersonalAccountContext>();
        var company = new CompanyModel()
        {
            Id = new Guid( companyId )
        };
        var branch = context.Branches.First(x => x.CompanyId == company.Id);
        var setting = new LoadingSettingsModel()
        {
            Branch = new BranchModel() { Id = branch.Id, Name = branch.Name, Company = company },
            BatchSize = 10,
            StartPosition = 0
        };

        // Действие и проверка
        Assert.DoesNotThrowAsync(async () =>
        {
            await repo.SaveAsync(setting, CancellationToken.None);
            var result = await repo.LoadAsync(branch.Id, CancellationToken.None);

            Assert.That(result.StartPosition == setting.StartPosition, Is.True);
        });
    }
}
