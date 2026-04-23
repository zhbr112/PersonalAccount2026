using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Репозиторий для работы с настройками загрузки данных
/// </summary>
public interface ICompanySettingsRepository
{
    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    public void Save(LoadingSettingsModel setting);

    /// <summary>
    /// Загрузить настройки.
    /// </summary>
    /// <param name="branchId">Уникальный код филиала.</param>
    /// <returns></returns>
    public LoadingSettingsModel Load(Guid branchId);

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task SaveAsync(LoadingSettingsModel setting,  CancellationToken token);

    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="branchId">Уникальный код филиала.</param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<LoadingSettingsModel> LoadAsync(Guid branchId, CancellationToken token);
}
