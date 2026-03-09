using PersonalAccount.Domain.Core;

namespace PersonalAccount.Common.Core;

/// <summary>
/// Абстрактный интерфейс репозиторий для наследования.
/// </summary>
public interface IRepository<T> where T: IDto;

