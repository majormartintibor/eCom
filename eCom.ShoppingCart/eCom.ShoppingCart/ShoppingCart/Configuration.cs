using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using Marten;
using Marten.Events.Projections;

namespace eCom.ShoppingCart.ShoppingCart;

public static class Configuration
{
    public static StoreOptions ConfigureShoppingCart(this StoreOptions options)
    {
        options.Projections.LiveStreamAggregation<Cart>();
        options.Projections.Add<CartDetailsProjection>(ProjectionLifecycle.Inline);

        return options;
    }
}