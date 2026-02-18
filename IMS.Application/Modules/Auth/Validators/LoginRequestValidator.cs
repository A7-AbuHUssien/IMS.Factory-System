using FluentValidation;
using IMS.Application.Modules.Auth.DTOs;

namespace IMS.Application.Modules.Auth.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.EmailUsername)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(30);
     
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6).WithMessage("Password must be at least 6 characters");

    }
}