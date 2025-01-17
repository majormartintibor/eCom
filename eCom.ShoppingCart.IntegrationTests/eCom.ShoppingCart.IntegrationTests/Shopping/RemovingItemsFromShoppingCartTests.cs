using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;
using Shouldly;

namespace eCom.ShoppingCart.IntegrationTests.Shopping;

public class RemovingItemsFromShoppingCartTests(AppFixture fixture) : GivenCartExists(fixture), IAsyncLifetime
{
    [Fact]
    public async Task Removing_an_existing_item_compeltely_removes_it_from_the_item_list()
    {
        _ = await Host.RemoveItem(
            new RemoveItem.RemoveItemRequest(
                Cart.Id,
                Cart.Items[0].ItemId,
                Cart.Items[0].Quantity));

        var cartResult = await Host.GetCartDetails(Cart.Id);
        var cart = await cartResult.ReadAsJsonAsync<CartDetails>();

        cart.Items.Count.ShouldBe(0);
    }

    [Fact]
    public async Task Removing_an_existing_item_decreases_quantity()
    {
        _ = await Host.RemoveItem(
            new RemoveItem.RemoveItemRequest(
                Cart.Id,
                Cart.Items[0].ItemId,
                Cart.Items[0].Quantity - 1));

        var cartResult = await Host.GetCartDetails(Cart.Id);
        var cart = await cartResult.ReadAsJsonAsync<CartDetails>();

        cart.Items[0].Quantity.ShouldBe(1);
    }
}