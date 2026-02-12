using FluentValidation;
using IMS.Application.Modules.Sales.DTOs.Customer;

namespace IMS.Application.Modules.Sales.Validators;
public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .When(x => x.Name != null);

        RuleFor(x => x.Email)
            .EmailAddress()
            .MaximumLength(150)
            .When(x => x.Email != null);

        RuleFor(x => x.Phone)
            .MaximumLength(20)
            .When(x => x.Phone != null);

        RuleFor(x => x.Address)
            .MaximumLength(500)
            .When(x => x.Address != null);
    }
}