using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Users.Queries.DeleteUser
{
    public sealed record DeleteUserCommand(Guid UserId) : IRequest<Result<Unit>>;
}
