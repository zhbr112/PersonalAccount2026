using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using PersonalAccount.Common.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Data.Logics;

/// <summary>
/// Реализация интерфейса <see cref="ILoadingSettingsRepository"/>
/// </summary>
public class LoadingSettingsRepository : ILoadingSettingsRepository
{
    /// <summary>
    /// Загрузить настройки
    /// </summary>
    /// <param name="company"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<LoadingSettings> Load(Company company, CancellationToken token)
    {
        var context = new PersonalAccountContext();
        var item = context.Companies.FirstOrDefault(x => x.Id == company.Id)
            ?? throw new InvalidDataException($"Не найдена организация по коду {company.Id}!");
        var json = !string.IsNullOrEmpty( item.LoadOptions ) ? item.LoadOptions
            :  throw new InvalidDataException($"Организация по коду {company.Id} содержит некорретные данные по настройкам!");

        var result = JsonSerializer.Deserialize< LoadingSettings >(json)
            ?? throw new InvalidDataException($"Организация по коду {company.Id} содержит некорретные данные по настройкам!");
        return result;
    }

    /// <summary>
    /// Сохранить настройки
    /// </summary>
    /// <param name="setting"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Task<bool> Save(LoadingSettings setting, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
