using FluentValidation;
using IMS.Application.Modules.Auth.DTOs.User;

namespace IMS.Application.Modules.Auth.Validators;


public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public  CreateUserDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        RuleFor(x => x.IsActive).NotNull();
    }
}