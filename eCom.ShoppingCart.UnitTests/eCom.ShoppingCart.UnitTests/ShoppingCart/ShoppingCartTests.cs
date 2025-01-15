using eCom.ShoppingCart.CreateCart;
using eCom.ShoppingCart.ShoppingCart;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.ShoppingCart;

public class ShoppingCartTests : BaseTest
{
    [Fact]
    public void CartCreated_creates_Cart()
    {
        Guid cartId = Guid.NewGuid();

        CartCreated cartCreated = new(cartId);

        var cart = Cart.Create(cartCreated);

        cart.Id.ShouldBe(cartId);
        cart.Items.ShouldBeEmpty();
    }

    [Fact]
    public void AddNewItem_adds_item()
    {
        Cart cart = TestData.EmptyCart(Guid.NewGuid());

        eCom.ShoppingCart.AddItem.ItemAdded itemAdded = 
            new(
                cart.Id, 
                Guid.NewGuid(), 
                _faker.Random.Int(1, TestData.DefaultItemQuantity - 1),
                _faker.Random.Int(1, TestData.DefaultItemUnitPrice));

        cart = cart.Apply(itemAdded);        

        cart.Items
            .Single()
            .ShouldBe(
                new Cart.Item(
                    itemAdded.ItemId, 
                    itemAdded.Quantity, 
                    itemAdded.UnitPrice));
    }

    [Fact]
    public void AddExisitingItem_increases_quantity()
    {
        Cart cart = TestData.CartWithOneItem();        
        int quantityToIncrease = _faker.Random.Int();

        eCom.ShoppingCart.AddItem.ItemAdded itemAdded =
            new(
                cart.Id,
                cart.Items.Single().ItemId,
                quantityToIncrease,
                cart.Items.Single().UnitPrice);

        cart = cart.Apply(itemAdded);

        cart.Items
            .Single()
            .Quantity
            .ShouldBe(TestData.DefaultItemQuantity + quantityToIncrease);
    }

    [Fact]
    public void AddNotExistingItem_to_CartWithItem_adds_NewItem()
    {
        Cart cart = TestData.CartWithOneItem();

        eCom.ShoppingCart.AddItem.ItemAdded itemAdded =
            new(
                cart.Id,
                Guid.NewGuid(),
                _faker.Random.Int(),
                _faker.Random.Int());

        cart = cart.Apply(itemAdded);

        cart.Items
            .First(i => i.ItemId == itemAdded.ItemId)
            .ShouldBe(new Cart.Item(
                itemAdded.ItemId, 
                itemAdded.Quantity, 
                itemAdded.UnitPrice));
    }

    [Fact]
    public void ReducingExistingItemQuantity_decreases_quantity()
    {
        Cart cart = TestData.CartWithOneItem();        
        int quantityToDecrease = _faker.Random.Int(1, TestData.DefaultItemQuantity - 1);

        eCom.ShoppingCart.RemoveItem.ItemRemoved itemRemoved =
            new(
                cart.Id,
                cart.Items.Single().ItemId,
                quantityToDecrease);

        cart = cart.Apply(itemRemoved);

        cart.Items
            .Single()
            .Quantity
            .ShouldBe(TestData.DefaultItemQuantity - quantityToDecrease);
    }

    [Fact]
    public void RemovingExistingItem_removes_Item()
    {
        Cart cart = TestData.CartWithOneItem();                

        eCom.ShoppingCart.RemoveItem.ItemRemoved itemRemoved =
            new(
                cart.Id,
                cart.Items.Single().ItemId,
                TestData.DefaultItemQuantity);

        cart = cart.Apply(itemRemoved);

        cart.Items.ShouldBeEmpty();
    }

    [Fact]
    public void ClearingCart_clears_Cart()
    {
        Cart cart = TestData.CartWithOneItem();

        eCom.ShoppingCart.ClearCart.CartCleared cartCleared 
            = new(cart.Id);

        cart = cart.Apply(cartCleared);

        cart.Items.ShouldBeEmpty();
    }
}