using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Контракт получения новых справочников по настройкам.
/// </summary>
public interface IReferenceDataBySettingsProvider
{
    /// <summary>
    /// Получить новых сотрудников по настройкам.
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <returns>Набор сотрудников.</returns>
    public IReadOnlyCollection<EmploeeModel> GetNewEmployees(LoadingSettingsModel settings);

    /// <summary>
    /// Получить новую номенклатуру по настройкам.
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <returns>Набор номенклатуры.</returns>
    public IReadOnlyCollection<NomenclatureModel> GetNewNomenclatures(LoadingSettingsModel settings);

    /// <summary>
    /// Получить новые группы по настройкам.
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <returns>Набор групп.</returns>
    public IReadOnlyCollection<CategoryModel> GetNewGroups(LoadingSettingsModel settings);

    /// <summary>
    /// Получить новых сотрудников по настройкам (асинхронно).
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Набор сотрудников.</returns>
    public Task<IReadOnlyCollection<EmploeeModel>> GetNewEmployeesAsync(
        LoadingSettingsModel settings,
        CancellationToken token);

    /// <summary>
    /// Получить новую номенклатуру по настройкам (асинхронно).
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Набор номенклатуры.</returns>
    public Task<IReadOnlyCollection<NomenclatureModel>> GetNewNomenclaturesAsync(
        LoadingSettingsModel settings,
        CancellationToken token);

    /// <summary>
    /// Получить новые группы по настройкам (асинхронно).
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Набор групп.</returns>
    public Task<IReadOnlyCollection<CategoryModel>> GetNewGroupsAsync(
        LoadingSettingsModel settings,
        CancellationToken token);
}
