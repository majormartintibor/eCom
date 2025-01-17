using eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;
using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using Shouldly;

namespace eCom.ShoppingCart.IntegrationTests.Shopping;

public class ShoppingTests(AppFixture fixture) : GivenCartExists(fixture), IAsyncLifetime
{
    [Fact]
    public async Task Adding_an_item_to_not_existing_cart_creates_cart()
    {
        AddItem.AddItemRequest request = new(
                null,
                Guid.NewGuid(),
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100));

        var result = await Host.AddItem(request);
        
        var cartResult = await Host.GetCartDetails(await result.GetCreatedId());        
        var cart = await cartResult.ReadAsJsonAsync<CartDetails>();

        cart.Items[0].ShouldBe(
            new ShoppingCart.Item(
                request.ItemId, 
                request.Quantity, 
                request.UnitPrize));
    }

    [Fact]
    public async Task Adding_an_item_to_an_existing_cart_increases_quantity()
    {
        AddItem.AddItemRequest request = new(
                Cart.Id,
                Cart.Items[0].ItemId,
                _faker.Random.Int(1, 100),
                Cart.Items[0].UnitPrice);

        var result = await Host.AddItem(request);

        var cartResult = await Host.GetCartDetails(await result.GetCreatedId());
        var cart = await cartResult.ReadAsJsonAsync<CartDetails>();

        cart.Items[0].ShouldBe(
            new ShoppingCart.Item(
                request.ItemId,
                Cart.Items[0].Quantity + request.Quantity,
                request.UnitPrize));
    }

    [Fact]
    public async Task Adding_a_new_item_to_an_existing_cart_adds_new_item()
    {
        AddItem.AddItemRequest request = new(
                Cart.Id,
                Guid.NewGuid(),
                _faker.Random.Int(1, 100),
                _faker.Random.Int(1, 100));

        var result = await Host.AddItem(request);

        var cartResult = await Host.GetCartDetails(await result.GetCreatedId());
        var cart = await cartResult.ReadAsJsonAsync<CartDetails>();
       
        cart.Items.FirstOrDefault(i => i.ItemId == request.ItemId).ShouldNotBeNull();
        cart.Items.Count.ShouldBe(2);
    }

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