using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Posts.Commands.DeletePost
{
    public sealed record DeletePostCommand(bool IsAdmin,Guid UserId, Guid PostId) : IRequest<Result<Unit>>;
}
