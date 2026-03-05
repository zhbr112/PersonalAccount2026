using System;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Репозиторий для работы с настройками загрузки данных
/// </summary>
public interface ILoadingSettingsRepository
{
    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<bool> Save(LoadingSettings setting,  CancellationToken token);

    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="company"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    public Task<LoadingSettings> Load(Company company, CancellationToken token);
}
