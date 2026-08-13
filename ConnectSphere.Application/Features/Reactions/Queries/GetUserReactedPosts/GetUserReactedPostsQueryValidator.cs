using ConnectSphere.Application.Common.Validators;
using FluentValidation;

namespace ConnectSphere.Application.Reactions.Queries.GetUserReactedPosts
{
    public class GetUserReactedPostsQueryValidator : AbstractValidator<GetUserReactedPostsQuery>
    {
        public GetUserReactedPostsQueryValidator()
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