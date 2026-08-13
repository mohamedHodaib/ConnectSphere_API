using ConnectSphere.Application.Posts.Commands.CreatePost;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Follows.Commands.Follows
{
    public class FollowsCommandValidator : AbstractValidator<FollowsCommand>
    {
        public FollowsCommandValidator()
        {
            RuleFor(x => x.FollowerId)
                .NotEmpty();

            RuleFor(x => x.FollowingId)
                .NotEmpty();
        }
    }
}
