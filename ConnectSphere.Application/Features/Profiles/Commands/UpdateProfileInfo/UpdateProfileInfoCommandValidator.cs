using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Settings;
using FluentValidation;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Profiles.Commands.UpdateProfileInfo
{
    public class UpdateProfileInfoCommandValidator : AbstractValidator<UpdateProfileInfoCommand>
    {
        public UpdateProfileInfoCommandValidator(IOptions<ValidationSettings> validationSettings)
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Post ID is required and cannot be empty."); ;

            RuleFor(x => x.UpdateProfileInfoDto)
                .NotNull()
                .WithMessage("Update date must be provided.");

            RuleFor(x => x.UpdateProfileInfoDto)
                .SetValidator(new UpdateProfileDtoValidator(validationSettings));
        }

        public class UpdateProfileDtoValidator : AbstractValidator<SettingsDto>
        {
            public UpdateProfileDtoValidator(IOptions<ValidationSettings> validationSettings)
            {
                RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(50);

                RuleFor(x => x.LastName)
                    .NotEmpty()
                    .MaximumLength(50);

                RuleFor(x => x.Biography)
                    .MaximumLength(500);

                RuleFor(x => x.Username)
                    .NotEmpty()
                    .Length(3, 30)
                    .Matches("^[a-zA-Z0-9._]+$")
                        .WithMessage("Username can only contain letters, numbers, dots, and underscores.");

                RuleFor(x => x.IsPrivate)
                    .NotNull();

                RuleFor(x => x.ShowOnlineStatus)
                    .NotNull();

                RuleFor(x => x.EmailNotificationsEnabled)
                    .NotNull();

                //Validate  image
                RuleFor(x => x.ProfilePicture)
                    .Must(image => image.Length > 0)
                    .When(x => x.ProfilePicture != null)
                    .WithMessage("Image cannot be empty.");
            }
        }
    }
}
