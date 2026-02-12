using FluentValidation;
using IMS.Application.Modules.Auth.DTOs.Role;

namespace IMS.Application.Modules.Auth.Validators;

public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50)
            .When(x => x.Name != null);
    }
}