using FluentValidation;

namespace ConnectSphere.Application.Features.Bookmarks.Commands.DeleteBookmark
{
    public class DeleteBookmarkCommandValidator : AbstractValidator<DeleteBookmarkCommand>
    {
        public DeleteBookmarkCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}