using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Validators;
using ConnectSphere.Application.Feed.Queries.GetFeed;
using ConnectSphere.Application.Recommendations.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Recommendations.Queries
{
    public class GetUserSuggestionsQueryValidator : AbstractValidator<GetUserSuggestionsQuery>
    {
        public GetUserSuggestionsQueryValidator()
        {
            // Validate the top-level property
            RuleFor(x => x.UserId)
                .NotEmpty();

            //Check if the nested DTO is null.
            RuleFor(x => x.PaginationParameters)
                .NotNull()
                .WithMessage("Pagination data must be provided.");

            // Set a validator for the DTO itself.
            RuleFor(x => x.PaginationParameters)
                .SetValidator(new PaginationDtoValidator());
        }

    }
}
