using ConnectSphere.Application.Common.Models;
using MediatR;
using System;

namespace ConnectSphere.Application.Features.Bookmarks.Commands.DeleteBookmark
{
    public record DeleteBookmarkCommand(Guid UserId, Guid PostId) : IRequest<Result<Unit>>;
}