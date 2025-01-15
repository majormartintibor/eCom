namespace eCom.ShoppingCart.AddItem;

public sealed record AddItem(
    Guid CartId, 
    Guid ItemId, 
    int Quantity, 
    decimal UnitPrice);