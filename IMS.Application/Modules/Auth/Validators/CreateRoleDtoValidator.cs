using FluentValidation;
using IMS.Application.Modules.Auth.DTOs.Role;

namespace IMS.Application.Modules.Auth.Validators;

public class CreateRoleDtoValidator : AbstractValidator<CreateRoleDto>
{
    public  CreateRoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);
    }
}