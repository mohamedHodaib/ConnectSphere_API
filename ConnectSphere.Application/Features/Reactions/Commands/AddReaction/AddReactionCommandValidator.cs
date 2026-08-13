using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Posts.Queries.GetPost;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Reactions.Commands.AddReaction
{
    public class AddReactionCommandValidator : AbstractValidator<AddReactionCommand>
    {
        public AddReactionCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.PostId)
                .NotEmpty();

            RuleFor(x => x.CreateReactionDto)
                .NotNull()
                .WithMessage("Reaction Data must be provided.");

            RuleFor(x => x.CreateReactionDto)
                .SetValidator(new CreateReactionDtoValidator());
        }
    }

    public class CreateReactionDtoValidator : AbstractValidator<CreateReactionDto>
    {
        public CreateReactionDtoValidator()
        {
            RuleFor(x => x.Type)
                .NotEmpty();
        }
    }
}
