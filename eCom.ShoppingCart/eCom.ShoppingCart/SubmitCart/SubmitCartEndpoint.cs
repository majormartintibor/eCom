using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine;
using Wolverine.Http;

namespace eCom.ShoppingCart.SubmitCart;

public static class SubmitCartEndpoint
{
    public const string Enpoint = "/api/shoppingcart/submit";

    [AllowAnonymous]
    [Tags("ShoppingCart")]
    [ProducesResponseType(200)]
    [WolverinePost(Enpoint)]
    public static async Task<IResult> Post(
        SubmitCartRequest request,
        IMessageBus bus)
    {
        await bus.InvokeAsync(new SubmitCart(request.CartId));

        return Results.Ok();
    }
}