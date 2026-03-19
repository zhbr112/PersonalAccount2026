using System;
using PersonalAccount.Common.Core;
using PersonalAccount.Data.Logics;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

public class LoadingService : ILoadingService
{
    public bool Push(CompanyModel company, IEnumerable<JournalRowDto> transactions, CancellationToken token)
    {
        // 1 Поучаем настройки
        var settingsRepo = new CompanySettingsRepository();
        var settings =  settingsRepo.LoadAsync( company, token ).Result
                        ?? new LoadingSettingsModel()
                        {
                            Owner = company, StartPosition = 1, BatchSize = 1000
                        };

        var firstTransaction = transactions.FirstOrDefault();
        if(firstTransaction is null) return false;
        
        // Отбрасываем лишние
        var innerTransactions = transactions.Where(x => x.Code >= settings.StartPosition);

        // Сохраняем 
        
        // Обновляем настройки
        var lastCode = innerTransactions.OrderByDescending(x => x.Code).First().Code;
        settings.StartPosition = lastCode;
        var task = Task.Run( () =>  settingsRepo.SaveAsync( settings , token), token);
        Task.WaitAll( task );
    
        return true;
    }


    public async Task<bool> PushAsync(CompanyModel company, IEnumerable<JournalRowDto> transactions, CancellationToken token)
        => await Task.Run( () => Push( company, transactions, token), token);
}
