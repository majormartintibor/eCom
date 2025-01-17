using Marten;
using Marten.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;

namespace eCom.ShoppingCart.ShoppingCart.Projections.CartDetails;

public static class GetDetailsEndpoint
{
    [AllowAnonymous]
    [Tags("ShoppingCart")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [WolverineGet("/api/shoppingcart/{cartId:guid}")]
    public static Task GetInspection([FromRoute] Guid cartId, IQuerySession querySession, HttpContext context)
        => querySession.Json.WriteById<CartDetails>(cartId, context);
}