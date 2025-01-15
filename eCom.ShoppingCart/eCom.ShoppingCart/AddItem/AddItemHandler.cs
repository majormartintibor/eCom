using eCom.ShoppingCart.ShoppingCart;
using Wolverine;
using Wolverine.Marten;

namespace eCom.ShoppingCart.AddItem;

public static class AddItemHandler
{
    [AggregateHandler]
    public static (Events, OutgoingMessages) Handle(
        AddItem command,
        Cart cart)
    {
        var events = new Events();
        var messages = new OutgoingMessages();

        events.Add(
            new ItemAdded(
                command.CartId, 
                command.ItemId, 
                command.Quantity, 
                command.UnitPrice));

        return new (events, messages);
    }
}