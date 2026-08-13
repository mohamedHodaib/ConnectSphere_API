using ConnectSphere.Application.Common.Models;
using MediatR;
using System;

namespace ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark
{
    public record CreateBookmarkCommand(Guid UserId, Guid PostId) : IRequest<Result<Unit>>;
}