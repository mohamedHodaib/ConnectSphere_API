using ConnectSphere.Application.Profiles.Queries.GetProfilePictureUrl;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Profiles.Queries.GetProfilePictureUrl
{
    public class GetProfilePictureUrlQueryValidator : AbstractValidator<GetProfilePictureUrlQuery>
    {
        public GetProfilePictureUrlQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
