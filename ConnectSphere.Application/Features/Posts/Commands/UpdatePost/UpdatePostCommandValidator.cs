using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Posts.Commands.UpdatePost;
using FluentValidation;

 namespace  ConnectSphere.Application.Posts.Commands.UpdatePost;
public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        // Validate the top-level property
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.PostId)
            .NotEmpty();

        //Check if the nested DTO is null.
        RuleFor(x => x.UpdatePostDto)
            .NotNull()
            .WithMessage("Post data must be provided.");

        // Set a validator for the DTO itself.
        RuleFor(x => x.UpdatePostDto)
            .SetValidator(new UpdatePostDtoValidator());
    }
}
// You create a separate validator for the nested DTO
public class UpdatePostDtoValidator : AbstractValidator<UpdatePostDto>
{
    public UpdatePostDtoValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
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
    }
}