using System;
using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PersonalAccount.Common.Core;
using PersonalAccount.Common.Models;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Console.Logics;

/// <summary>
/// Фоновый процесс для загрузки данных журнала
/// </summary>
public class BackgroungPushService : BackgroundService
{
    private readonly ConsoleOptions _options;
    private readonly IClientRepository<JournalRowDto> _repo;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<BackgroungPushService> _logger;

    public BackgroungPushService(
            ConsoleOptions options,
            IClientRepository<JournalRowDto> repo,
            IHostApplicationLifetime hostApplicationLifetime,
            ILogger<BackgroungPushService> logger)
    {
        _options = options;
        _repo = repo;
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var client = new HttpClient();
            var apiUrl = BuildApiUrl();

            _logger.LogInformation("Запуск фоновой выгрузки. CompanyId={CompanyId}, BranchId={BranchId}", _options.CompanyId, _options.BranchId);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Получаем текущие настройки загрузки.
                var response = await client.GetAsync(apiUrl, stoppingToken);
                response.EnsureSuccessStatusCode();

                var settings = await response.Content.ReadFromJsonAsync<LoadingSettingsModel>(cancellationToken: stoppingToken);
                if (settings is null)
                    throw new InvalidOperationException("Невозможно получить текущие настройки для загрузки данных!");

                // Загружаем пачку и завершаем задачу, когда новых записей нет.
                using var connect = new SqlConnection(_options.ConnectionString);
                var transactions = (await _repo.GetRows(connect, settings)).ToArray();

                if (transactions.Length == 0)
                {
                    _logger.LogInformation("Фоновая выгрузка завершена: новых данных не найдено.");
                    break;
                }

                var pushResponse = await client.PostAsJsonAsync(apiUrl, transactions, stoppingToken);
                pushResponse.EnsureSuccessStatusCode();

                _logger.LogInformation(
                    "Отправлена пачка: count={Count}, from={FromCode}, to={ToCode}",
                    transactions.Length,
                    transactions.Min(x => x.Code),
                    transactions.Max(x => x.Code));
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Фоновая выгрузка остановлена по запросу отмены.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка выполнения фоновой выгрузки.");
        }
        finally
        {
            // Это одноразовая задача: после завершения закрываем приложение.
            _hostApplicationLifetime.StopApplication();
        }
    }

    private string BuildApiUrl()
    {
        if (_options.BranchId == Guid.Empty)
            return $"{_options.ServerHost}/console/{_options.CompanyId}";

        return $"{_options.ServerHost}/console/{_options.CompanyId}/{_options.BranchId}";
    }
}
