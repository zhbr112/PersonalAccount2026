using Microsoft.EntityFrameworkCore;
using PersonalAccount.Common.Core;
using PersonalAccount.Data;
using PersonalAccount.Domain.Models;
using PersonalAccount.Domain.Models.Dto;

namespace PersonalAccount.Api.Logics;

/// <summary>
/// Реализация интерфейса <see cref="ILoadingService"/>
/// </summary>
public class LoadingService : ILoadingService
{
    // Репозиторий для работы с настройками загрузки данных
    private readonly  ICompanySettingsRepository _settingReposity;

    // Репозиторий для скоростной записи данных в журнал
    private readonly IServerRepository<JournalRowDto> _writerRepository;

    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context;
  
    public LoadingService( 
            PersonalAccountContext context,
            ICompanySettingsRepository settingsRepository, 
            IServerRepository<JournalRowDto> writerRepository)
    {
        _context = context;
        _settingReposity = settingsRepository;
        _writerRepository = writerRepository;
    }        
       
       
    /// <InhericDoc/>
    public bool Push(CompanyModel company, IEnumerable<JournalRowDto> transactions, CancellationToken token)
    {
        // 1 Получаем настройки
        var settings = _settingReposity.Load( company) 
                        ?? new LoadingSettingsModel()
                        {
                            Owner = company, StartPosition = 1, BatchSize = 1000
                        };

        var firstTransaction = transactions.FirstOrDefault();
        if(firstTransaction is null) return false;
        
        // Отбрасываем лишние
        var innerTransactions = transactions.Where(x => x.Code >= settings.StartPosition);

        // Сохраняем 
        var connect = _context.Database.GetDbConnection();
        var writeTask = _writerRepository.SaveRows(connect, innerTransactions, settings );
        
        // Обновляем настройки
        var lastCode = innerTransactions.OrderByDescending(x => x.Code).First().Code;
        settings.StartPosition = lastCode;
        _settingReposity.Save( settings );
    
        return true;
    }

    /// <InhericDoc/>
    public async Task<bool> PushAsync(CompanyModel company, IEnumerable<JournalRowDto> transactions, CancellationToken token)
        => await Task.Run( () => Push( company, transactions, token), token);
}
