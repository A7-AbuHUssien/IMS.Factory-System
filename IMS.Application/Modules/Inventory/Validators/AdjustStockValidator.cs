using FluentValidation;
using IMS.Application.Modules.Inventory.DTOs.Stock;

namespace IMS.Application.Modules.Inventory.Validators;

public class AdjustStockValidator : AbstractValidator<AdjustStockDto>
{
    public AdjustStockValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required.");

        RuleFor(x => x.ActualQuantity)
            .GreaterThanOrEqualTo(0).WithMessage("Actual quantity cannot be negative.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason for adjustment is required.")
            .MinimumLength(5).WithMessage("Please provide a detailed reason (min 5 chars).");
    }
}