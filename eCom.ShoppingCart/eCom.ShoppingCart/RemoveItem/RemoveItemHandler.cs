using eCom.ShoppingCart.ShoppingCart;
using Wolverine.Marten;
using Wolverine;

namespace eCom.ShoppingCart.RemoveItem;

public static class RemoveItemHandler
{
    [AggregateHandler]
    public static (Events, OutgoingMessages) Handle(
        RemoveItem command,
        Cart cart)
    {
        var events = new Events();
        var messages = new OutgoingMessages();

        if (cart.Items.FirstOrDefault(i => i.ItemId == command.ItemId) is null)
        {
            throw new InvalidOperationException("can not remove a non-existing item!");
        }

        events.Add(
            new ItemRemoved(
                command.CartId, 
                command.ItemId, 
                command.Quantity));

        return new(events, messages);
    }
}