using eCom.ShoppingCart.AddItem;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.AddItem;
public class AddItemTests : BaseTest
{
    [Fact]
    public void Handling_AddItemCommand_Should_Return_ItemAddedEvent()
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
    public void Handle_AddItemCommand_ShouldReturn_ExpectedItemAddedEvent()
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