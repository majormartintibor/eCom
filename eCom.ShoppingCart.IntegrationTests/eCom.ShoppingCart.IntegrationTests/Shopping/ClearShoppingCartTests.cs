using eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;
using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using Shouldly;

namespace eCom.ShoppingCart.IntegrationTests.Shopping;

public class ClearShoppingCartTests(AppFixture fixture) : GivenCartExists(fixture), IAsyncLifetime
{
    [Fact]
    public async Task Clearing_a_cart_clears_item_list()
    {
        _ = await Host.ClearCart(
            new ClearCart.ClearCartRequest(Cart.Id));

        var cartResult = await Host.GetCartDetails(Cart.Id);
        var cart = await cartResult.ReadAsJsonAsync<CartDetails>();

        cart.Items.Count.ShouldBe(0);
    }
}
