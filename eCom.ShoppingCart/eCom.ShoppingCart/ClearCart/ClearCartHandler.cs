using Wolverine.Marten;
using Wolverine;
using eCom.ShoppingCart.ShoppingCart;

namespace eCom.ShoppingCart.ClearCart;

public static class ClearCartHandler
{
    [AggregateHandler]
    public static (Events, OutgoingMessages) Handle(
        ClearCart command,
        Cart cart)
    {
        Events events = [];
        OutgoingMessages messages = [];

        events.Add(
            new CartCleared(command.CartId));

        return new(events, messages);
    }
}