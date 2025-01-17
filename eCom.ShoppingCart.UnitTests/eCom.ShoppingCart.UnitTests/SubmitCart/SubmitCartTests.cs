using eCom.Contracts.ShoppingCart;
using eCom.ShoppingCart.SubmitCart;
using eCom.ShoppingCart.TestBase;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.SubmitCart;

public class SubmitCartTests : BaseTest
{
    [Fact]
    public void Submitting_a_cart_submits_cart()
    {        
        var cart = TestData.CartWithOneItem();
        var command = new eCom.ShoppingCart.SubmitCart.SubmitCart(cart.Id);

        var (events, messages) = SubmitCartHandler.Handle(command, cart);

        events.Single().ShouldBeOfType<CartSubmitted>();
        messages.Single().ShouldBeOfType<CartPublished>();
    }

    [Fact]
    public void Resubmitting_should_fail()
    {
        var cart = TestData.SubmittedCart();
        var command = new eCom.ShoppingCart.SubmitCart.SubmitCart(cart.Id);

        Should.Throw<InvalidOperationException>(() => SubmitCartHandler.Handle(command, cart));
    }

    [Fact]
    public void Submitting_an_empty_cart_should_fail()
    {
        var cart = TestData.EmptyCart(Guid.NewGuid());
        var command = new eCom.ShoppingCart.SubmitCart.SubmitCart(cart.Id);

        Should.Throw<InvalidOperationException>(() => SubmitCartHandler.Handle(command, cart));
    }
}