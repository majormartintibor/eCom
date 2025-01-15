using Bogus;

namespace eCom.ShoppingCart.UnitTests;

public abstract class BaseTest 
{
    protected readonly Faker _faker;

    protected BaseTest()
    {
        _faker = new();
    }
}