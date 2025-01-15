namespace eCom.ShoppingCart.AddItem;

public sealed record ItemAdded(Guid CartId, Guid ItemId, int Quantity, decimal UnitPrice);