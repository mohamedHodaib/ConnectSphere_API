using ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark;
using FluentValidation;

namespace ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark
{
    public class CreateBookmarkCommandValidator : AbstractValidator<CreateBookmarkCommand>
    {
        public CreateBookmarkCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PostId).NotEmpty();
        }
    }
}