using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Posts.Commands.CreatePost
{
    public sealed record AddCommentCommand(Guid UserId ,CreatePostDto CreatePostDto) : IRequest<Result<PostDetailsDto>>;
}
