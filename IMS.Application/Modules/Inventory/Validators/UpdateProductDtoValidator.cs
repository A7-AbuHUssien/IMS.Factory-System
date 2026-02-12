using FluentValidation;
using IMS.Application.Modules.Inventory.DTOs.Product;

namespace IMS.Application.Modules.Inventory.Validators;

public class UpdateProductDtoValidator 
    : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {
        When(x => x.Name != null, () =>
        {
            RuleFor(x => x.Name)
                .MaximumLength(150);
        });

        When(x => x.UnitPrice.HasValue, () =>
        {
            RuleFor(x => x.UnitPrice.Value)
                .GreaterThan(0);
        });
    }
}
