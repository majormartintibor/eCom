using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;

namespace eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;

public class GivenSubmittedCartExists(AppFixture fixture) : GivenCartExists(fixture), IAsyncLifetime
{
    public override async Task InitializeAsync()
    {
        Cart = await Host.CreatedCart();
        SubmittedCart = await Host.SubmittedCart();
        EmptyCart = await Host.CreateEmpty();
    }

    public CartDetails SubmittedCart { get; protected set; } = default!;
    public CartDetails EmptyCart { get; protected set; } = default!;
}