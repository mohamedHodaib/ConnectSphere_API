using ConnectSphere.Application.Comments.Queries.ViewComments;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Validators;
using ConnectSphere.Application.Feed.Queries.GetFeed;
using ConnectSphere.Application.Posts.Commands.CreatePost;
using ConnectSphere.Application.Reactions.Commands.AddReaction;
using FluentValidation;

namespace ConnectSphere.Application.Comments.Queries.ViewComments;
public class ViewCommentsQueryValidator : AbstractValidator<ViewCommentsQuery>
{
    public ViewCommentsQueryValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.PaginationDto)
            .SetValidator(new PaginationDtoValidator());
    }

}
