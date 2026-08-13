using ConnectSphere.Application.Common.Validators;
using FluentValidation;

namespace ConnectSphere.Application.Features.Bookmarks.Queries.GetUserBookmarks
{
    public class GetUserBookmarksQueryValidator : AbstractValidator<GetUserBookmarksQuery>
    {
        public GetUserBookmarksQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();

            RuleFor(x => x.PaginationDto)
                .NotNull()
                .WithMessage("Pagination data must be provided.");

            RuleFor(x => x.PaginationDto)
                .SetValidator(new PaginationDtoValidator());
        }
    }
}