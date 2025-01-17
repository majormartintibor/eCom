using eCom.ShoppingCart.AddItem;
using eCom.ShoppingCart.TestBase;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.AddItem;
public class AddItemTests : BaseTest
{
    [Fact]
    public void Handling_additem_command_should_return_itemadded_event()
    {
        eCom.ShoppingCart.AddItem.AddItem command 
            = new(
                default,
                default,
                default,
                default);

        var (events, _) = AddItemHandler.Handle(command, TestData.EmptyCart(command.CartId));

        events.Single().ShouldBeOfType<ItemAdded>();
    }

    [Fact]
    public void Handle_additem_command_should_return_expected_itemadded_event()
    {
        eCom.ShoppingCart.AddItem.AddItem command
            = new(
                Guid.NewGuid(),
                Guid.NewGuid(),
                _faker.Random.Int(),
                _faker.Random.Int());

        ItemAdded expected = new(
            command.CartId,
            command.ItemId,
            command.Quantity,
            command.UnitPrice);

        var (events, _) = AddItemHandler.Handle(command, TestData.EmptyCart(command.CartId));

        events.Single().ShouldBe(expected);       
    }    
}