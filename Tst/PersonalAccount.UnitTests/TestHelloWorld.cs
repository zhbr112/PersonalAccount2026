using NUnit.Framework;
using PersonalAccount.Domain;

namespace PersonalAccount.UnitTests;

[TestFixture]
public class TestHelloWorld
{
    [Test]
    public void Check_HelloWorld()
    {
        // Подготовка
        var class1 = new Class1();

        // Действие
        var result = class1.GetHelloWorld();

        // Проверки
        Assert.That( string.IsNullOrWhiteSpace(result) == false);
    }

    [Test]
    public void FailCheck_HelloWorld()
    {
        Assert.Fail("Test failed!");    
    }
}
