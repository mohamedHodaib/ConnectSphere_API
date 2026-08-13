using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Posts.Commands.UpdatePost
{
    public sealed record UpdatePostCommand(Guid UserId,Guid PostId, UpdatePostDto UpdatePostDto) : IRequest<Result<Unit>>;
}
