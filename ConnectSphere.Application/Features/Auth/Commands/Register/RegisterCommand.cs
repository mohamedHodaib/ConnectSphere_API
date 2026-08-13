using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Auth.Commands.Register
{
    public sealed record RegisterCommand(RegisterDto RegisterDto) : IRequest<Result<UserProfileDetailsDto>>;
}
