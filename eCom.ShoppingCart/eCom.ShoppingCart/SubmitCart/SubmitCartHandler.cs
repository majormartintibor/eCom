using Wolverine.Marten;
using Wolverine;
using eCom.ShoppingCart.ShoppingCart;
using eCom.Contracts.ShoppingCart;

namespace eCom.ShoppingCart.SubmitCart;

public static class SubmitCartHandler
{
    [AggregateHandler]
    public static (Events, OutgoingMessages) Handle(
        SubmitCart command,
        Cart cart)
    {
        Events events = [];
        OutgoingMessages messages = [];

        if (cart.IsSubmitted)
        {
            throw new InvalidOperationException("Can not resubmit cart!");
        }

        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("Can not submit empty cart!");
        }

        events.Add(new CartSubmitted(command.CartId));
        messages.Add(new CartPublished(command.CartId));

        return new(events, messages);
    }
}