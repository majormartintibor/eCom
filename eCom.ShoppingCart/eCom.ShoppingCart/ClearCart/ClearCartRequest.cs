using FluentValidation;

namespace eCom.ShoppingCart.ClearCart;

public sealed record ClearCartRequest(Guid CartId)
{
    public sealed class ClearCardRequestValidator : AbstractValidator<ClearCartRequest>
    {
        public ClearCardRequestValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
        }
    }
}