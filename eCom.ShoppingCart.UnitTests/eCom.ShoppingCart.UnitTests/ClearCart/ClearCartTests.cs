using eCom.ShoppingCart.AddItem;
using eCom.ShoppingCart.ClearCart;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.ClearCart;

public class ClearCartTests : BaseTest
{
    [Fact]
    public void Handling_ClearCartCommand_Should_Return_CartCleared()
    {
        eCom.ShoppingCart.ClearCart.ClearCart command = new(default);

        var (events, _) = ClearCartHandler.Handle(command, TestData.EmptyCart(command.CartId));

        events.Single().ShouldBeOfType<CartCleared>();
    }

    [Fact]
    public void Handle_ClearCartCommand_ShouldReturn_ExpectedCartClearedEvent()
    {
        Guid cartId = Guid.NewGuid();
        eCom.ShoppingCart.ClearCart.ClearCart command = new(cartId);
        CartCleared expected = new(cartId);

        var (events, _) = ClearCartHandler.Handle(command, TestData.EmptyCart(command.CartId));

        events.Single().ShouldBe(expected);
    }
}
