using ConnectSphere.Application.Profiles.Queries.GetOwnSettings;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Profiles.Queries.GetOwnSettings
{
    public class GetOwnSettingsQueryValidator : AbstractValidator<GetOwnSettingsQuery>
    {
        public GetOwnSettingsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
