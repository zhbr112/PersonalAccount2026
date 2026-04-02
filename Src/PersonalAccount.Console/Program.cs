using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Console.Extensions;
using PersonalAccount.Domain;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

// Лого
CurrentApplication.ShowLogo();

// Настройки приложения и сервисы
var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

var services = new ServiceCollection()
                    .RegistryPersonalAccountConsole(configuration);
var provider = services.BuildServiceProvider();
var options = provider.GetRequiredService<ConsoleOptions>();

var client = new HttpClient();

// Получаем текущие настройки для организации
var url =  $"{options.ServerHost}/console/{options.CompanyId}" ;

var response = await client.GetAsync( url );
var settings = await response.Content.ReadFromJsonAsync<LoadingSettingsModel>();
if (settings is null) throw new InvalidOperationException("Невозможно получить текущие настройки для загрузки данных!");
Console.WriteLine($"[{DateTime.Now}] Starting fetching data for company: {settings.Owner.Name}, batch: {settings.BatchSize}, start: {settings.StartPosition}");

// Загружаем первую пачку
var repo = provider.GetRequiredService< IClientRepository<JournalRowDto> >();
using var connect = new SqlConnection(options.ConnectionString);
var transactions = await repo.GetRows( connect, settings );

// Передаем данные пока получаем транзакции
while (transactions.Count() > 0)
{
    // 1. Передали транзакции
    await client.PostAsJsonAsync( url, transactions );
    
    // 2. Запросили новые настройки
    response = await client.GetAsync( url );
    settings = await response.Content.ReadFromJsonAsync<LoadingSettingsModel>();
    if (settings is null) throw new InvalidOperationException("Невозможно получить текущие настройки для загрузки данных!");

    // 3. Загрузили новую пачку
    transactions = await repo.GetRows( connect, settings );           
}

Console.WriteLine($"[{DateTime.Now}] Finished fetching data. Current position: {settings.StartPosition}");

