namespace eCom.ShoppingCart.RemoveItem;

public sealed record RemoveItem(
    Guid CartId, 
    Guid ItemId, 
    int Quantity);