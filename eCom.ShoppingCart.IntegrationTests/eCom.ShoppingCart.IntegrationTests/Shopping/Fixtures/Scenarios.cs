using Alba;
using Bogus;
using eCom.ShoppingCart.AddItem;
using eCom.ShoppingCart.ClearCart;
using eCom.ShoppingCart.RemoveItem;
using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using Microsoft.AspNetCore.Authentication;
using Shouldly;
using System.Text;

namespace eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;

public static class Scenarios
{
    internal static readonly Faker _faker = new();

    public static Task<IScenarioResult> CreateCart(
        this IAlbaHost api) =>
            api.Scenario(x =>
            {
                x.Post.Url(AddItemEndpoint.Enpoint);
                x.Post.Json(new AddItemRequest(
                    null, 
                    Guid.NewGuid(), 
                    _faker.Random.Int(2, 100), 
                    _faker.Random.Int(1, 100)));

                x.StatusCodeShouldBe(200);
            });

    public static async Task<CartDetails> CreatedCart(
        this IAlbaHost api)
    {
        var result = await api.CreateCart();

        result = await api.GetCartDetails(await result.GetCreatedId()); 

        var cart = await result.ReadAsJsonAsync<CartDetails>();

        cart.ShouldNotBeNull();

        return cart;
    }

    public static Task<IScenarioResult> GetCartDetails(
        this IAlbaHost api,
        Guid cartId
    ) =>
        api.Scenario(x =>
        {
            x.Get.Url($"/api/shoppingcart/{cartId}");
            x.IgnoreStatusCode();
        });

    public static Task<IScenarioResult> AddItem(
        this IAlbaHost api,
        AddItemRequest request) =>
            api.Scenario(x =>
            {
                x.Post.Url(AddItemEndpoint.Enpoint);
                x.Post.Json(request);

                x.StatusCodeShouldBe(200);
            });

    public static Task<IScenarioResult> RemoveItem(
        this IAlbaHost api,
        RemoveItemRequest request) =>
            api.Scenario(x =>
            {
                x.Post.Url(RemoveItemEndpoint.Enpoint);
                x.Post.Json(request);

                x.StatusCodeShouldBe(200);
            });

    public static Task<IScenarioResult> ClearCart(
        this IAlbaHost api,
        ClearCartRequest request) =>
            api.Scenario(x =>
            {
                x.Post.Url(ClearCartEndpoint.Enpoint);
                x.Post.Json(request);

                x.StatusCodeShouldBe(200);
            });

    public static async Task<Guid> GetCreatedId(this IScenarioResult result)
    {
        return await result.ReadAsJsonAsync<Guid>();
    }
}