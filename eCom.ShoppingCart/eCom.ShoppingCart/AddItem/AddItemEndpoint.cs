using Wolverine;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using eCom.ShoppingCart.CreateCart;
using Marten;
using eCom.ShoppingCart.ShoppingCart;

namespace eCom.ShoppingCart.AddItem;

public static class AddItemEndpoint
{
    public const string Enpoint = "/api/shoppingcart/additem";

    [Tags("ShoppingCart")]
    [ProducesResponseType(200)]
    [WolverinePost(Enpoint)]
    public static async Task<IResult> Post(
        AddItemRequest request,
        IMessageBus bus,
        IDocumentSession documentSession
       )
    {
        Guid shoppingCartId = request.CartId ?? Guid.NewGuid();
        //todo: enrich with user data?

        if (request.CartId is null)
        {
            documentSession.Events
                .StartStream<Cart>(shoppingCartId, new CartCreated(shoppingCartId));

            await documentSession.SaveChangesAsync();
        }

        await bus.InvokeAsync(
            new AddItem(
                shoppingCartId,
                request.ItemId,
                request.Quantity,
                request.UnitPrize));

        return Results.Ok(shoppingCartId);
    }
}