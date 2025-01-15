namespace eCom.ShoppingCart.RemoveItem;

public sealed record ItemRemoved(Guid CartId, Guid ItemId, int Quantity);