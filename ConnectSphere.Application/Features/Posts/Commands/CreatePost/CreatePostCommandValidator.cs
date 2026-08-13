using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Posts.Commands.CreatePost;
using FluentValidation;

public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        // Validate the top-level property
        RuleFor(x => x.UserId)
            .NotEmpty();

        //Check if the nested DTO is null.
        RuleFor(x => x.CreatePostDto)
            .NotNull()
            .WithMessage("Post data must be provided.");

        // Set a validator for the DTO itself.
        RuleFor(x => x.CreatePostDto)
            .SetValidator(new CreatePostDtoValidator());
    }
}
// You create a separate validator for the nested DTO
public class CreatePostDtoValidator : AbstractValidator<CreatePostDto>
{
    public CreatePostDtoValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(10000);

        //Validate the Tags as whole.
        RuleFor(x => x.Tags)
            .Must(tags => tags!.Count <= 20)
            .When(x => x.Tags != null);

        //Validate each tag.
        RuleForEach(x => x.Tags)
            .NotEmpty() 
            .WithMessage("Individual tag names cannot be empty.")
            .MaximumLength(50);

        //Validate post images
        RuleFor(x => x.Images)
        .Must(images => images == null || images.Count <= 100)
        .WithMessage("A post cannot contain more than 100 images.");

        //Validate each post image
        RuleForEach(x => x.Images)
        .Must(image => image.Length > 0)
        .WithMessage("Image cannot be empty.");

    }
}