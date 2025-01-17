using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;

namespace eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;

public class GivenCartExists(AppFixture fixture) : IntegrationContext(fixture), IAsyncLifetime
{
    public override async Task InitializeAsync()
    {
        Cart = await Host.CreatedCart();        
    }

    public CartDetails Cart { get; protected set; } = default!;   
}