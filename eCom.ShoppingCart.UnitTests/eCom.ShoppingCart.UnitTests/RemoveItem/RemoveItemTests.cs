using eCom.ShoppingCart.RemoveItem;
using eCom.ShoppingCart.TestBase;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.RemoveItem;

public class RemoveItemTests : BaseTest
{
    [Fact]
    public void Handling_removeitem_command_should_return_itemremoved_event()
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
    public void Handle_additem_command_should_return_expected_itemadded_event()
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
    public void Handle_additem_command_with_wrong_item_id_throws_invalidoperationexception()
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
