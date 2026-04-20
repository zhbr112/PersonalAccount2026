using System.Text.Json;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Data.Logics;

/// <summary>
/// Реализация интерфейса <see cref="ICompanySettingsRepository"/>
/// </summary>
public class CompanySettingsRepository : ICompanySettingsRepository
{
    // Контекст для работы с базой данных
    private readonly PersonalAccountContext _context;

    /// <summary>
    /// Создать обхект типа <see cref="CompanySettingsRepository"/>
    /// </summary>
    /// <param name="context"> Контекст для работы с базой данных </param>
    public CompanySettingsRepository(PersonalAccountContext context) => _context = context;

    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="branchId"> Уникальный код филиала. </param>
    /// <returns></returns>
    public LoadingSettingsModel Load(Guid branchId)
    {
        var item = _context.Branches
            .Where(x => x.Id == branchId)
            .Join(
                _context.Companies,
                branch => branch.CompanyId,
                company => company.Id,
                (branch, company) => new { Branch = branch, Company = company })
            .FirstOrDefault()
            ?? throw new InvalidDataException($"Не найден филиал по коду {branchId}!");

        var json = !string.IsNullOrEmpty(item.Branch.LoadOptions) ? item.Branch.LoadOptions
            : throw new InvalidDataException($"Филиал по коду {branchId} содержит некорретные данные по настройкам!");

        var result = JsonSerializer.Deserialize< LoadingSettingsModel >(json)
            ?? throw new InvalidDataException($"Филиал по коду {branchId} содержит некорретные данные по настройкам!");
        return result;
    }


    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="branchId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<LoadingSettingsModel> LoadAsync(Guid branchId, CancellationToken token)
        => await Task.Run(() => Load(branchId), token);

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"> Настройки </param>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task SaveAsync(LoadingSettingsModel setting, CancellationToken token)
        => await Task.Run( () => Save(setting), token);

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    public void Save(LoadingSettingsModel setting)
    {
        var branchId = setting.Branch?.Id ?? throw new InvalidDataException("Невозможно сохранить настройки т.к. нет информации о филиале!");
        var branch = _context.Branches.FirstOrDefault(x => x.Id == branchId)
            ?? throw new InvalidDataException($"Не найден филиал по коду {branchId}!");

        var text = JsonSerializer.Serialize(setting);
        branch.LoadOptions = text;
        _context.SaveChanges();
    }
}
