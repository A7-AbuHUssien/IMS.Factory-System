using FluentValidation;
using IMS.Application.Modules.Inventory.DTOs.Warehouse;

namespace IMS.Application.Modules.Inventory.Validators;

public class UpdateWarehouseDtoValidator : AbstractValidator<UpdateWarehouseDto>
{
    public UpdateWarehouseDtoValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(150)
            .When(x => x.Name != null);

        RuleFor(x => x.Location)
            .MaximumLength(250)
            .When(x => x.Location != null);
    }
   
}