using FluentValidation;

namespace eCom.ShoppingCart.SubmitCart;

public sealed record SubmitCartRequest(Guid CartId)
{
    public sealed class SubmitCartRequestValidator : AbstractValidator<SubmitCartRequest>
    {
        public SubmitCartRequestValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
        }
    }
}