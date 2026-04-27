using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Универсальный контракт получения новых данных по настройкам.
/// </summary>
/// <typeparam name="TData">Тип получаемых данных.</typeparam>
public interface INewDataBySettingsProvider<TData>
{
    /// <summary>
    /// Получить набор новых данных по настройкам.
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <returns>Набор новых данных.</returns>
    public IReadOnlyCollection<TData> GetNew(LoadingSettingsModel settings);

    /// <summary>
    /// Получить набор новых данных по настройкам (асинхронно).
    /// </summary>
    /// <param name="settings">Настройки инкрементальной выборки.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Набор новых данных.</returns>
    public Task<IReadOnlyCollection<TData>> GetNewAsync(
        LoadingSettingsModel settings,
        CancellationToken token);
}
