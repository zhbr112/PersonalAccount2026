using System;

namespace PersonalAccount.Common.Models;

/// <summary>
/// Настройки для Api
/// </summary>
public class ApiOptions
{
    /// <summary>
    /// Строка подключения к Postgres
    /// </summary>
    public string ConnectionString { get; set; } = null!;
}
