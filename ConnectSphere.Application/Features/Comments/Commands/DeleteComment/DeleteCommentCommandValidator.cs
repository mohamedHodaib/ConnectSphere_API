using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Posts.Commands.CreatePost;
using ConnectSphere.Application.Reactions.Commands.AddReaction;
using FluentValidation;

namespace ConnectSphere.Application.Comments.Commands.DeleteComment;
public class DeleteCommentQueryValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentQueryValidator()
    {
        RuleFor(x => x.userId)
               .NotEmpty();

        RuleFor(x => x.PostId)
               .NotEmpty();

        RuleFor(x => x.CommentId)
                .NotEmpty();
    }

}