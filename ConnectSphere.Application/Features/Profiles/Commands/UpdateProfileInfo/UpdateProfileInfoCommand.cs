using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Profiles.Commands.UpdateProfileInfo
{
    public sealed record UpdateProfileInfoCommand(Guid UserId,SettingsDto UpdateProfileInfoDto) 
        : IRequest<Result<Unit>>;
}
