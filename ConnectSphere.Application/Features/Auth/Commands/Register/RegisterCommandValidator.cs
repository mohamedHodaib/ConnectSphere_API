using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Settings;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace ConnectSphere.Application.Auth.Commands.Register
{
    // The validator still targets the Command
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        // Settings are now passed to the DTO Validator, simplifying this class
        public RegisterCommandValidator()
        {
            // 1. Ensure the nested DTO is not null (Safety check)
            RuleFor(x => x.RegisterDto)
                .NotNull()
                .WithMessage("Registration data must be provided.");

            // 2. Delegate the actual validation rules to the DTO validator
            // The DTO Validator will be instantiated and its dependencies (validationSettings) resolved automatically by FluentValidation's DI integration.
            RuleFor(x => x.RegisterDto)
                .SetValidator(new RegisterDtoValidator());
        }
    }


    // The validator now targets the DTO itself
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {

        // Dependency injection is still required here for settings
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty();

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }

    }

}