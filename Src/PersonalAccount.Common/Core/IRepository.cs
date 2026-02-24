using System.Data.Common;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Интерфейс для реализации репозитория команд для загрузки данных из внешних баз данных.
/// </summary>
public interface IRepository<T> where T: IDto
{
    /// <summary>
    /// Получить пакет данных согласно настройкам.
    /// </summary>
    /// <param name="connection"> Абстрактный коннект. </param>
    /// <param name="options"> Настройки. </param>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetRows(DbConnection connection, LoadingSettings options);
}
