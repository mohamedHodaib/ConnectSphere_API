using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Comments.Commands.AddComment
{
    public sealed record AddCommentCommand(Guid CommentedUserId,Guid PostId,AddCommentDto AddCommentDto)
        : IRequest<Result<CommentDetailsDto>>;
}
