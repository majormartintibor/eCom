using FluentValidation;

namespace eCom.ShoppingCart.AddItem;

public sealed record AddItemRequest(
    Guid? CartId, 
    Guid ItemId, 
    int Quantity, 
    decimal UnitPrize)
{
    public sealed class AddItemRequestValidator : AbstractValidator<AddItemRequest>
    {
        public AddItemRequestValidator()
        {            
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Quantity).GreaterThan(0);            
        }
    }
};