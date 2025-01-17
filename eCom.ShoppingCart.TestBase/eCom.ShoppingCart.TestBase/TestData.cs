using Bogus;
using eCom.ShoppingCart.CreateCart;
using eCom.ShoppingCart.ShoppingCart;
using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;

namespace eCom.ShoppingCart.TestBase;

public static class TestData
{
    public const int DefaultItemQuantity = 10;
    public const int DefaultItemUnitPrice = 10;

    internal static readonly Faker _faker = new();

    public static Cart EmptyCart(Guid cartId)
        => new(cartId, []);

    public static Cart CartWithOneItem()
        => new(
            Guid.NewGuid(), 
            [new Item(
                Guid.NewGuid(), 
                DefaultItemQuantity, 
                DefaultItemUnitPrice)]);    

    public static CartDetails CreateCartDetails()
        => new (Guid.NewGuid(), [], default);

    public static CartDetails CartDetailsWithItem(CartDetails cartDetails)
        => CartDetailsProjection.Apply(
            new AddItem.ItemAdded(
                cartDetails.Id, 
                Guid.NewGuid(),
                _faker.Random.Int(1,100),
                _faker.Random.Int(1,100)), cartDetails);

    public static Item Item(Guid ItemId)
        => new(ItemId, _faker.Random.Int(1, 100), _faker.Random.Int(1, 100));
}
