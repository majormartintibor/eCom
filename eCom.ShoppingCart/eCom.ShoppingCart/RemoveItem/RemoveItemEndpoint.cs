﻿using Microsoft.AspNetCore.Mvc;
using Wolverine.Http;
using Wolverine;
using Microsoft.AspNetCore.Authorization;

namespace eCom.ShoppingCart.RemoveItem;

public static class RemoveItemEndpoint
{
    public const string Enpoint = "/api/shoppingcart/removeitem";

    [AllowAnonymous]
    [Tags("ShoppingCart")]
    [ProducesResponseType(200)]
    [WolverinePost(Enpoint)]
    public static async Task<IResult> Post(
        RemoveItemRequest request,
        IMessageBus bus
       )
    {
        await bus.InvokeAsync(
            new RemoveItem(
                request.CartId,
                request.ItemId,
                request.Quantity));

        return Results.Ok();
    }
}