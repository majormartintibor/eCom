using eCom.ShoppingCart.IntegrationTests.Shopping.Fixtures;
using eCom.ShoppingCart.SubmitCart;

namespace eCom.ShoppingCart.IntegrationTests.Shopping;

public class SubmittingCartTests(AppFixture fixture) : GivenSubmittedCartExists(fixture), IAsyncLifetime
{
    [Fact]
    public async Task Submitting_a_cart_succeeds()
    {
        await Host.SubmitCart(new SubmitCartRequest(Cart.Id));
    }

    [Fact]
    public async Task Resubmitting_should_fail()
    {
        await Host.Scenario(x =>
        {
            x.Post.Url(SubmitCartEndpoint.Enpoint);
            x.Post.Json(new SubmitCartRequest(SubmittedCart.Id));

            x.StatusCodeShouldBe(500);
        });
    }

    [Fact]
    public async Task Submitting_an_empty_cart_should_fail()
    {
        await Host.Scenario(x =>
        {
            x.Post.Url(SubmitCartEndpoint.Enpoint);
            x.Post.Json(new SubmitCartRequest(EmptyCart.Id));

            x.StatusCodeShouldBe(500);
        });
    }
}