using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Posts.Commands.CreatePost;
using ConnectSphere.Application.Reactions.Commands.AddReaction;
using FluentValidation;

namespace ConnectSphere.Application.Comments.Commands.AddComment;
public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.CommentedUserId)
               .NotEmpty();

        RuleFor(x => x.PostId)
               .NotEmpty();

        RuleFor(x => x.AddCommentDto)
                .NotNull()
                .WithMessage("Comment Data must be provided.");

        RuleFor(x => x.AddCommentDto)
            .SetValidator(new AddCommentDtoValidator());
    }

}


public class AddCommentDtoValidator : AbstractValidator<AddCommentDto>
{
    public AddCommentDtoValidator()
    {
        

        RuleFor(x => x.Content)
            .NotEmpty();
    }
}
