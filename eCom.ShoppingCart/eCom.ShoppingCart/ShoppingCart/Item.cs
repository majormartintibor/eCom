namespace eCom.ShoppingCart.ShoppingCart;

public sealed record Item(Guid ItemId, int Quantity, decimal UnitPrice)
{
    public Item AddQuantity(int quantity)
        => this with { Quantity = Quantity + quantity };
}