using ConnectSphere.Application.Features.Profiles.Queries.GetPublicProfileInfo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Profiles.Queries.GetPublicProfileInfo
{
    public class GetPublicProfileInfoQueryValidator : AbstractValidator<GetPublicProfileInfoQuery>
    {
        public GetPublicProfileInfoQueryValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
