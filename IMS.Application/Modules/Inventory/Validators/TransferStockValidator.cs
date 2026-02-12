using FluentValidation;
using IMS.Application.Modules.Inventory.DTOs.Stock;

namespace IMS.Application.Modules.Inventory.Validators;

public class TransferStockValidator : AbstractValidator<TransferStockDto>
{
    public TransferStockValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.SourceWarehouseId).NotEmpty();
        RuleFor(x => x.DestinationWarehouseId).NotEmpty()
            .NotEqual(x => x.SourceWarehouseId).WithMessage("Source and Destination warehouses cannot be the same.");
            
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Transfer quantity must be greater than 0.");
    }
}