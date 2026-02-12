using FluentValidation;
using IMS.Application.Modules.Sales.DTOs.SalesOrder;

namespace IMS.Application.Modules.Sales.Validators;

public class CreateSalesOrderDtoValidator : AbstractValidator<CreateSalesOrderDto>
{
    public CreateSalesOrderDtoValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty();

        RuleFor(x => x.Items)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Order must contain at least one item");

        RuleFor(x => x.TaxAmount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Discount)
            .GreaterThanOrEqualTo(0);
    }
}