using System;

namespace PersonalAccount.Common.Models;

/// <summary>
/// Настройки для Console
/// </summary>
public class ConsoleOptions
{
    /// <summary>
    /// Строка подключения к MSSQL
    /// </summary>
    public string ConnectionString { get; set; } = null!;
}
