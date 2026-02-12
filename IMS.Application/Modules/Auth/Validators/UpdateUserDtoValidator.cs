using FluentValidation;
using IMS.Application.Modules.Auth.DTOs.User;

namespace IMS.Application.Modules.Auth.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserDtoValidator()
    {
        RuleFor(e=>e.Id).NotNull().NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).MaximumLength(100).When(x => x.Name != null);
        RuleFor(x => x.Email).EmailAddress().MaximumLength(255).When(x => x.Email != null);
        RuleFor(x => x.Password).MinimumLength(6).When(x => x.Password != null);
    }
}