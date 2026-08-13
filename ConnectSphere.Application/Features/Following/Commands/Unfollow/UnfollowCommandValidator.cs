using ConnectSphere.Application.Follows.Commands.Unfollow;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Following.Commands.Unfollow
{
    public class UnfollowCommandValidator : AbstractValidator<UnfollowCommand>
    {
        public UnfollowCommandValidator()
        {
            RuleFor(x => x.FollowerId)
                .NotEmpty();

            RuleFor(x => x.FollowingId)
                .NotEmpty();

            RuleFor(x => x).Must(x => x.FollowerId != x.FollowingId)
               .WithMessage("Follower id and following id must be different.");
        }
    }
}
