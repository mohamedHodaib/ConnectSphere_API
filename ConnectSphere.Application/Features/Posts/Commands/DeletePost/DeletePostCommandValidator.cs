using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Posts.Commands.DeletePost;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Posts.Commands.DeletePost
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            // Validate the top-level property
            RuleFor(x => x.UserId)
                .NotEmpty();

            //Validate the PostId
            RuleFor(x => x.PostId)
                .NotEmpty();
        }
    }
}
