using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using MediatR;
using System;

namespace ConnectSphere.Application.Features.Bookmarks.Queries.GetUserBookmarks
{
    public record GetUserBookmarksQuery(Guid UserId, PaginationParameters PaginationDto) 
        : IRequest<Result<PagedResult<PostDetailsDto>>>;
}