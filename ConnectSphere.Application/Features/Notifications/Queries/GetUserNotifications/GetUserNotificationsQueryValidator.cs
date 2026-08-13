using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Validators;
using ConnectSphere.Application.Features.Notifications.Queries.GetUserNotifications;
using ConnectSphere.Application.Feed.Queries.GetFeed;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryValidator : AbstractValidator<GetUserNotificationsQuery>
    {
        public GetUserNotificationsQueryValidator()
        {
            // Validate the top-level property
            RuleFor(x => x.UserId)
                .NotEmpty();

            //Check if the nested DTO is null.
            RuleFor(x => x.PaginationDto)
                .NotNull()
                .WithMessage("Pagination data must be provided.");

            // Set a validator for the DTO itself.
            RuleFor(x => x.PaginationDto)
                .SetValidator(new PaginationDtoValidator());
        }
    }
}