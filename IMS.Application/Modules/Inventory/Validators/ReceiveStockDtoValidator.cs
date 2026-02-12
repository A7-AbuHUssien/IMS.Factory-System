using FluentValidation;
using IMS.Application.Modules.Inventory.DTOs.Stock;

namespace IMS.Application.Modules.Inventory.Validators;

public class ReceiveStockDtoValidator : AbstractValidator<ReceiveStockDto>
{
    public ReceiveStockDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Received quantity must be greater than 0.");

        RuleFor(x => x.UnitCost)
        .GreaterThan(0).WithMessage("Received quantity must be greater than 0.");
        RuleFor(x => x.Reference)
            .MaximumLength(100);
    }
}