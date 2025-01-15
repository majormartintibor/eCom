using eCom.ShoppingCart.AddItem;
using eCom.ShoppingCart.ClearCart;
using eCom.ShoppingCart.CreateCart;
using eCom.ShoppingCart.RemoveItem;
using static eCom.ShoppingCart.ShoppingCart.Cart;

namespace eCom.ShoppingCart.ShoppingCart;

public sealed record Cart(Guid Id, List<Item> Items)
{
    public static Cart Create(CartCreated created) => new(created.Id, []);

    public Cart Apply(ItemAdded itemAdded) =>
        this with { Items = AddToItems(Items, itemAdded) };

    public Cart Apply(ItemRemoved itemRemoved) =>
        this with { Items = RemoveFromItems(Items, itemRemoved) };

    public Cart Apply(CartCleared cartCleared) =>
        this with { Items = [] };

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

    public sealed record Item(Guid ItemId, int Quantity, decimal UnitPrice)
    {
        public Item AddQuantity(int quantity)
            => this with { Quantity = Quantity + quantity };
    }
}