using FluentValidation;

namespace ConnectSphere.Application.Posts.Queries.GetPost
{
    public class GetPostQueryValidator : AbstractValidator<GetPostQuery>
    {
        public GetPostQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Post ID is required and cannot be empty.");
        }
    }
}