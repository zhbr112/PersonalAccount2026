namespace PersonalAccount.Domain.Core;

/// <summary>
/// Набор типов транзакции.
/// </summary>
public enum TransactionType : long
{
    /// <summary>
    /// Продажа.
    /// </summary>
    Sale = 1 ,

    /// <summary>
    /// Возврат.
    /// </summary>
    Return = 2,

    /// <summary>
    /// Сдача
    /// </summary>
    Change = 3,

    /// <summary>
    /// Начала рабочей смены.
    /// </summary>
    StartShift = 4,

    /// <summary>
    /// Окончание рабочей смены.
    /// </summary>
    StopShift = 5
}
