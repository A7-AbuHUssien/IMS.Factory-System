using FluentValidation;
using IMS.Application.Modules.Sales.DTOs.Order;

namespace IMS.Application.Modules.Sales.Validators;

public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
{
    public CreateOrderDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();
    }
}