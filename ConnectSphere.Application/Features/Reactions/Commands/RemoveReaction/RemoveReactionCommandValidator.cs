using ConnectSphere.Application.Common.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Reactions.Commands.RemoveReaction
{
    public class RemoveReactionCommandValidator : AbstractValidator<RemoveReactionCommand>
    {
        public RemoveReactionCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.PostId)
                .NotEmpty();
        }
    }
}
