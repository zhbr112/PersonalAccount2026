namespace PersonalAccount.Common.Core;

/// <summary>
/// Контракт записи полученных данных.
/// </summary>
/// <typeparam name="TData">Тип записываемых данных.</typeparam>
public interface IReceivedDataWriter<TData>
{
    /// <summary>
    /// Записать полученные данные.
    /// </summary>
    /// <param name="data">Набор данных для записи.</param>
    /// <returns>Признак успешной записи.</returns>
    public bool Write(IReadOnlyCollection<TData> data);

    /// <summary>
    /// Записать полученные данные (асинхронно).
    /// </summary>
    /// <param name="data">Набор данных для записи.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Признак успешной записи.</returns>
    public Task<bool> WriteAsync(
        IReadOnlyCollection<TData> data,
        CancellationToken token);
}
