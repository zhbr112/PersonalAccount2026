using System;
using NUnit.Framework;
using PersonalAccount.Domain;

namespace PersonalAccount.UnitTests;


/*
Имя проверяемого метода
Сценарий, в котором тестируется метод
Ожидаемое поведение при вызове сценария
*/


/// <summary>
/// Набор модульных тестов в рамках приложения.
/// </summary>
public class ApplicationTests
{
    /// <summary>
    /// Проверить получение версии сборки приложения.
    /// </summary>
    [Test]
    public void CurrentVersion_Show_Any()
    {
        // Подготовка
        var version = CurrentApplication.CurrentVersion();

        // Действие

        // Проверка
        Assert.That(!string.IsNullOrEmpty(version));
    }
}
