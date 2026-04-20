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
    private bool Push(BranchModel branch, IEnumerable<JournalRowDto> transactions)
    {
        // 1 Получаем настройки
        var settings = _settingReposity.Load(branch.Id)
                        ?? new LoadingSettingsModel()
                        {
                            Branch = branch, StartPosition = 1, BatchSize = 1000
                        };

        var firstTransaction = transactions.FirstOrDefault();
        if(firstTransaction is null) return false;
        
        // Отбрасываем лишние
        var innerTransactions = transactions.Where(x => x.Code >= settings.StartPosition);

        // Сохраняем 
        var connect = _context.Database.GetDbConnection();
        var task = _writerRepository.SaveRows(connect, innerTransactions, settings );
        
        // Обновляем настройки
        var lastCode = innerTransactions.OrderByDescending(x => x.Code).First().Code;
        settings.StartPosition = lastCode;
        _settingReposity.Save( settings );

        Task.WaitAll( task );
        return true;
    }

    /// <InhericDoc/>
    public bool Push(Guid branchId, IEnumerable<JournalRowDto> transactions)
    {
        var branch = _context.Branches
            .Include(x => x.Company)
            .FirstOrDefault(x => x.Id == branchId)
            ?? throw new InvalidOperationException($"Невозможно получить карточку филиала по коду {branchId}!");
        return Push(
            new BranchModel()
            {
                Id = branchId,
                Company = new CompanyModel()
                {
                    Id = branch.CompanyId,
                    Name = branch.Company.Name ?? string.Empty,
                    Address = branch.Company.Address ?? string.Empty,
                    INN = branch.Company.Inn ?? string.Empty
                },
                Name = branch.Name
            },
            transactions);
    }

    /// <InhericDoc/>
    public async Task<bool> PushAsync(Guid branchId, IEnumerable<JournalRowDto> transactions, CancellationToken token)
        => await Task.Run(() => Push(branchId, transactions), token);

    /// <InhericDoc/>
    public LoadingSettingsModel GetSettings(Guid branchId)
    {
        var branch = _context.Branches
            .Include(x => x.Company)
            .FirstOrDefault(x => x.Id == branchId)
            ?? throw new InvalidOperationException($"Невозможно получить карточку филиала по коду {branchId}!");
        
        // Конвертируем в модель
        var companyModel = new CompanyModel()
        { 
            Id = branch.CompanyId,
            Name = branch.Company.Name ?? string.Empty,
            Address = branch.Company.Address ?? string.Empty,
            INN = branch.Company.Inn ?? string.Empty
        };

        var branchModel = new BranchModel()
        {
            Id = branchId,
            Name = branch.Name,
            Company = companyModel
        };

        var settings = _settingReposity.Load(branchId);

         // Сформируем новый набор настроек и сохраним их.
        if (settings is null)
        {
           
            settings = new LoadingSettingsModel()
                        {
                            Branch = branchModel, StartPosition = 1, BatchSize = 1000
                        };
            _settingReposity.Save( settings );            
        }

        return settings;
    }

    /// <InhericDoc/>
    public async Task<LoadingSettingsModel> GetSettingsAsync(Guid branchId, CancellationToken token)
        => await Task.Run(() => GetSettings(branchId), token);

}
