using ConnectSphere.Application.Common.DTOs;
using FluentValidation;

namespace ConnectSphere.Application.Auth.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
           
            RuleFor(x => x.LoginDto)
                .NotNull()
                .WithMessage("Login data object cannot be null.");

            RuleFor(x => x.LoginDto)
                .SetValidator(new LoginDtoValidator());
        }
    }

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("A valid email address is required for login."); 

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password must be not empty.");
        }
    }
}