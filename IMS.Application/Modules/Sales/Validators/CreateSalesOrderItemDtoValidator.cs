using FluentValidation;
using IMS.Application.Modules.Sales.DTOs.SalesOrderItem;

namespace IMS.Application.Modules.Sales.Validators;

public class CreateSalesOrderItemDtoValidator : AbstractValidator<CreateSalesOrderItemDto>
{
    public CreateSalesOrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}
