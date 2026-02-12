using FluentValidation;
using IMS.Application.Modules.Sales.DTOs.SalesOrder;

namespace IMS.Application.Modules.Sales.Validators;

public class UpdateSalesOrderItemDtoValidator : AbstractValidator<UpdateSalesOrderDto>
{
    public UpdateSalesOrderItemDtoValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        When(x => x.TaxAmount.HasValue, () =>
        {
            RuleFor(x => x.TaxAmount.Value)
                .GreaterThanOrEqualTo(0);
        });

        When(x => x.Discount.HasValue, () =>
        {
            RuleFor(x => x.Discount.Value)
                .GreaterThanOrEqualTo(0);
        });
    }
}
