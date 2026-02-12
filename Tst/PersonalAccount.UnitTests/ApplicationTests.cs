using System;
using System.Reflection;
using NUnit.Framework;
using PersonalAccount.Domain;
using PersonalAccount.Domain.Core;
using PersonalAccount.Domain.Models;

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

    /// <summary>
    /// Проверяеи создание категории с наименование 1= null;
    /// </summary>
    [Test]
    public void Create_Category_CheckNullName()
    {
        // Подготовка
        var domain = new Category();

        // Действие

        // Проверка
        Assert.That (domain.Name is not null);
    }

    /// <summary>
    /// Проверяем наличие системных атрибутов.
    /// </summary>
    [Test]
    public void Create_Category_ExistsAttributes()
    {
        // Подготовка
        var type = typeof(Category);

        // Действие
        var properties = type.GetProperties().Where(x => x.GetCustomAttributes(true).Any());

        // Проверка
        Assert.That(properties.Any());
    }

    /// <summary>
    /// Проверить наличите атрибута <see cref="PhoneTemplateAttribute"/>
    /// </summary>
    [Test]
    public void Create_Emploee_ExistsPhoneTemplateAttribute()
    {
        // Подготовка
        var domain = new Emploee();

        // Действие
        var properties = domain.GetType()
                        .GetProperties()
                        .Where(x => x.GetCustomAttribute<PhoneTemplateAttribute>(true) is not null );

        // Проверки
        Assert.That(properties.Any());        
        var attribute = properties.First().GetCustomAttribute< PhoneTemplateAttribute >();
        Assert.That(!string.IsNullOrEmpty( attribute!.Template));
    }
}
