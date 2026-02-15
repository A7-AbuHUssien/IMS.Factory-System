using FluentValidation;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;

namespace IMS.Application.Modules.Sales.Validators;

public class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0);

        RuleFor(x => x.Quantity)
            .GreaterThan(0);
    }
}
