using eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;
using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using Shouldly;

namespace eCom.ShoppingCart.IntegrationTests.Shopping;

public class AddingItemsToShoppingCartTests(AppFixture fixture) : GivenCartExists(fixture), IAsyncLifetime
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
}