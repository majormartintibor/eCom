using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;

namespace eCom.ShoppingCart.ClearCart;

public static class ClearCartEndpoint
{
    public const string Enpoint = "/api/shoppingcart/clear";

    [AllowAnonymous]
    [Tags("ShoppingCart")]
    [ProducesResponseType(200)]
    [WolverinePost(Enpoint)]
    public static async Task<IResult> Post(
        ClearCartRequest request,
        IMessageBus bus)
    {
        await bus.InvokeAsync(new ClearCart(request.CartId));

        return Results.Ok();
    }
}
