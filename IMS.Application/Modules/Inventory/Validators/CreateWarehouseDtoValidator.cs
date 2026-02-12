using FluentValidation;
using IMS.Application.Modules.Inventory.DTOs.Warehouse;

namespace IMS.Application.Modules.Inventory.Validators;

public class CreateWarehouseDtoValidator :  AbstractValidator<CreateWarehouseDto>
{
    public CreateWarehouseDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.Location)
            .NotEmpty()
            .MaximumLength(250);
    }
}