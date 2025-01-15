using eCom.ShoppingCart.ShoppingCart;

namespace eCom.ShoppingCart.UnitTests;
internal static class TestData
{
    public const int DefaultItemQuantity = 10;
    public const int DefaultItemUnitPrice = 10;

    internal static Cart EmptyCart(Guid cartId)
        => new(cartId, []);

    internal static Cart CartWithOneItem()
        => new(
            Guid.NewGuid(), 
            [new Cart.Item(
                Guid.NewGuid(), 
                DefaultItemQuantity, 
                DefaultItemUnitPrice)]);
}
