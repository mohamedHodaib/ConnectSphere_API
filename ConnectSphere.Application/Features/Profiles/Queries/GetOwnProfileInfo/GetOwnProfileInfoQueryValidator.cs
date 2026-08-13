using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Profiles.Queries.GetOwnProfileInfo
{
    public class DeleteUserCommandValidator : AbstractValidator<GetOwnProfileInfoQuery>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("Post ID is required and cannot be empty."); 
        }
    }
}
