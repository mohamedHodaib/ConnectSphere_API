using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Reactions.Commands.RemoveReaction
{
    public sealed record RemoveReactionCommand(Guid UserId, Guid PostId) : IRequest<Result<Unit>>;
}
