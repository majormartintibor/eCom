using eCom.ShoppingCart.CreateCart;
using eCom.ShoppingCart.ShoppingCart;
using eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;
using eCom.ShoppingCart.TestBase;
using Shouldly;

namespace eCom.ShoppingCart.UnitTests.ShoppingCart;

public class CartDetailsTests : BaseTest
{
    [Fact]
    public void CartCreated_creates_CartDetail()
    {
        Guid cartId = Guid.NewGuid();

        CartCreated cartCreated = new(cartId);

        var cart = CartDetailsProjection.Create(cartCreated);

        cart.Id.ShouldBe(cartId);
        cart.Items.ShouldBeEmpty();
    }

    [Fact]
    public void Add_new_item_adds_item()
    {        
        CartDetails cart =  TestData.CreateCartDetails();

        eCom.ShoppingCart.AddItem.ItemAdded itemAdded =
            new(
                cart.Id,
                Guid.NewGuid(),
                _faker.Random.Int(1, TestData.DefaultItemQuantity - 1),
                _faker.Random.Int(1, TestData.DefaultItemUnitPrice));

        cart = CartDetailsProjection.Apply(itemAdded, cart);

        cart.Items
            .Single()
            .ShouldBe(
                new Item(
                    itemAdded.ItemId,
                    itemAdded.Quantity,
                    itemAdded.UnitPrice));

        cart.TotalPrice.ShouldBe(cart.Items.Sum(item => item.Quantity * item.UnitPrice));
    }

    [Fact]
    public void Add_exisiting_item_increases_quantity()
    {
        CartDetails cart = TestData.CartDetailsWithItem(TestData.CreateCartDetails());
        int origQuantity = cart.Items.Single().Quantity;
        int quantityToIncrease = _faker.Random.Int(1,100);

        eCom.ShoppingCart.AddItem.ItemAdded itemAdded =
            new(
                cart.Id,
                cart.Items.Single().ItemId,
                quantityToIncrease,
                cart.Items.Single().UnitPrice);

        cart = CartDetailsProjection.Apply(itemAdded, cart);

        cart.Items
            .Single()
            .Quantity
            .ShouldBe(origQuantity + quantityToIncrease);

        cart.TotalPrice.ShouldBe(cart.Items.Sum(item => item.Quantity * item.UnitPrice));
    }

    [Fact]
    public void Add_not_existing_item_to_cart_with_item_adds_new_item()
    {
        CartDetails cart = TestData.CartDetailsWithItem(TestData.CreateCartDetails());

        eCom.ShoppingCart.AddItem.ItemAdded itemAdded =
            new(
                cart.Id,
                Guid.NewGuid(),
                _faker.Random.Int(),
                _faker.Random.Int());

        cart = CartDetailsProjection.Apply(itemAdded, cart);

        cart.Items
            .First(i => i.ItemId == itemAdded.ItemId)
            .ShouldBe(new Item(
                itemAdded.ItemId,
                itemAdded.Quantity,
                itemAdded.UnitPrice));

        cart.TotalPrice.ShouldBe(cart.Items.Sum(item => item.Quantity * item.UnitPrice));
    }

    [Fact]
    public void Reducing_existing_item_quantity_decreases_quantity()
    {
        CartDetails cart = TestData.CartDetailsWithItem(TestData.CreateCartDetails());
        int origQuantity = cart.Items.Single().Quantity;
        int quantityToDecrease = _faker.Random.Int(1, cart.Items.Single().Quantity - 1);

        eCom.ShoppingCart.RemoveItem.ItemRemoved itemRemoved =
            new(
                cart.Id,
                cart.Items.Single().ItemId,
                quantityToDecrease);

        cart = CartDetailsProjection.Apply(itemRemoved, cart);

        cart.Items
            .Single()
            .Quantity
            .ShouldBe(origQuantity - quantityToDecrease);

        cart.TotalPrice.ShouldBe(cart.Items.Sum(item => item.Quantity * item.UnitPrice));
    }

    [Fact]
    public void Removing_existing_item_removes_item()
    {
        CartDetails cart = TestData.CartDetailsWithItem(TestData.CreateCartDetails());

        eCom.ShoppingCart.RemoveItem.ItemRemoved itemRemoved =
            new(
                cart.Id,
                cart.Items.Single().ItemId,
                cart.Items.Single().Quantity);

        cart = CartDetailsProjection.Apply(itemRemoved, cart);

        cart.Items.ShouldBeEmpty();
        cart.TotalPrice.ShouldBe(default);
    }

    [Fact]
    public void Clearing_cart_clears_cart()
    {
        CartDetails cart = TestData.CartDetailsWithItem(TestData.CreateCartDetails());

        eCom.ShoppingCart.ClearCart.CartCleared cartCleared
            = new(cart.Id);

        cart = CartDetailsProjection.Apply(cartCleared, cart);

        cart.Items.ShouldBeEmpty();
    }

    [Fact]
    public void Cleared_carts_total_price_is_zero()
    {
        CartDetails cart = TestData.CartDetailsWithItem(TestData.CreateCartDetails());

        eCom.ShoppingCart.ClearCart.CartCleared cartCleared
            = new(cart.Id);

        cart = CartDetailsProjection.Apply(cartCleared, cart);

        cart.TotalPrice.ShouldBe(default);
    }
}