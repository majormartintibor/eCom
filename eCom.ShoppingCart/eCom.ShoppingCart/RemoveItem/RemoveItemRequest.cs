using FluentValidation;

namespace eCom.ShoppingCart.RemoveItem;

public sealed record RemoveItemRequest(
    Guid CartId, 
    Guid ItemId, 
    int Quantity)
{
    public sealed class RemoveItemRequestValidator : AbstractValidator<RemoveItemRequest>
    {
        public RemoveItemRequestValidator()
        {
            RuleFor(x => x.CartId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
};