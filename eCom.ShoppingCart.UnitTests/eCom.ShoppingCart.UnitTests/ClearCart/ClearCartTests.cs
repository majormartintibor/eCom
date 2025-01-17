using eCom.ShoppingCart.ClearCart;
using eCom.ShoppingCart.TestBase;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.ClearCart;

public class ClearCartTests : BaseTest
{
    [Fact]
    public void Handling_clear_cart_command_should_return_cartcleared()
    {
        eCom.ShoppingCart.ClearCart.ClearCart command = new(default);

        var (events, _) = ClearCartHandler.Handle(command, TestData.EmptyCart(command.CartId));

        events.Single().ShouldBeOfType<CartCleared>();
    }

    [Fact]
    public void Handle_clearcart_command_should_return_expected_cartcleared_Event()
    {
        Guid cartId = Guid.NewGuid();
        eCom.ShoppingCart.ClearCart.ClearCart command = new(cartId);
        CartCleared expected = new(cartId);

        var (events, _) = ClearCartHandler.Handle(command, TestData.EmptyCart(command.CartId));

        events.Single().ShouldBe(expected);
    }
}
