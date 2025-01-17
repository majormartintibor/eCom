using eCom.ShoppingCart.AddItem;
using eCom.ShoppingCart.ClearCart;
using eCom.ShoppingCart.CreateCart;
using eCom.ShoppingCart.RemoveItem;
using Marten.Events.Aggregation;

namespace eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;

public sealed record CartDetails(
    Guid Id,
    List<Item> Items,
    int Version = 1)
{
    public decimal TotalPrice =>
        Items.Sum(item => item.Quantity * item.UnitPrice);
}

public sealed class CartDetailsProjection : SingleStreamProjection<CartDetails>
{
    public static CartDetails Create(CartCreated created) =>
        new(created.Id, [], default);

    public static CartDetails Apply(ItemAdded itemAdded, CartDetails current) =>
        current with
        {
            Items = AddToItems(current.Items, itemAdded)
        };

    public static CartDetails Apply(ItemRemoved itemRemoved, CartDetails current) =>
        current with
        {
            Items = RemoveFromItems(current.Items, itemRemoved)
        };

    public static CartDetails Apply(CartCleared cartCleared, CartDetails current) =>
        current with
        {
            Items = []           
        };

    private static List<Item> AddToItems(List<Item> items, ItemAdded itemAdded)
    {
        var updatedItems = items.Select(i => i.ItemId == itemAdded.ItemId
                                            ? i.AddQuantity(itemAdded.Quantity)
                                            : i).ToList();

        return updatedItems.Any(i => i.ItemId == itemAdded.ItemId)
               ? updatedItems
               : [.. updatedItems, new Item(itemAdded.ItemId, itemAdded.Quantity, itemAdded.UnitPrice)];
    }

    private static List<Item> RemoveFromItems(List<Item> items, ItemRemoved itemRemoved)
    {
        //We trust the data stored in our system, validation that a non-existing item
        //cannot be removed happened in the AggregateHandler
        var existingItem = items.First(i => i.ItemId == itemRemoved.ItemId);

        var updatedItem = existingItem.AddQuantity(-itemRemoved.Quantity);

        return updatedItem.Quantity > 0
            ? items.Select(i => i.ItemId == itemRemoved.ItemId ? updatedItem : i).ToList()
            : items.Where(i => i.ItemId != itemRemoved.ItemId).ToList();
    }
}