using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Follows.Commands.Follows
{
    public sealed record FollowsCommand(Guid FollowerId,Guid FollowingId) : IRequest<Result<Unit>>;
}
