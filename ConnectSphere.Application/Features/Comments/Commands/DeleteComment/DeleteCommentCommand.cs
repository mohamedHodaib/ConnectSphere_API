using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Comments.Commands.DeleteComment
{
    public sealed record DeleteCommentCommand(Guid userId,Guid PostId,Guid CommentId)
        : IRequest<Result<Unit>>;
}
