using eCom.ShoppingCart.RemoveItem;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.RemoveItem;

public class RemoveItemTests : BaseTest
{
    [Fact]
    public void Handling_RemoveItemCommand_Should_Return_ItemRemovedEvent()
    {
        var cart = TestData.CartWithOneItem();

        eCom.ShoppingCart.RemoveItem.RemoveItem removeItem =
            new(
                cart.Id,
                cart.Items.First().ItemId,
                default);

        var (events, _) = RemoveItemHandler.Handle(removeItem , cart);

        events.Single().ShouldBeOfType<ItemRemoved>();
    }

    [Fact]
    public void Handle_AddItemCommand_ShouldReturn_ExpectedItemAddedEvent()
    {
        var cart = TestData.CartWithOneItem();
        int quantity = _faker.Random.Int(1, TestData.DefaultItemQuantity - 1);

        eCom.ShoppingCart.RemoveItem.RemoveItem removeItem =
            new(
                cart.Id,
                cart.Items.First().ItemId,
                quantity);

        ItemRemoved expected = new(cart.Id, cart.Items.First().ItemId, quantity);

        var (events, _) = RemoveItemHandler.Handle(removeItem, cart);

        events.Single().ShouldBe(expected);        
    }

    [Fact]
    public void Handle_AddItemCommand_WithWrongItemId_Throws_InvalidOperationException()
    {
        var cart = TestData.CartWithOneItem();

        eCom.ShoppingCart.RemoveItem.RemoveItem removeItem =
            new(
                cart.Id,
                Guid.NewGuid(),
                default);

        Should.Throw<InvalidOperationException>(() => RemoveItemHandler.Handle(removeItem, cart));            
    }
}
