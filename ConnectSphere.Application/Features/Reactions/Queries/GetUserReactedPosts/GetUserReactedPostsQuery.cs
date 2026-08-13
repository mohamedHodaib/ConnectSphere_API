using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using MediatR;
using System;

namespace ConnectSphere.Application.Reactions.Queries.GetUserReactedPosts
{
    public record GetUserReactedPostsQuery(Guid UserId, PaginationParameters PaginationDto)
        : IRequest<Result<PagedResult<PostDetailsDto>>>;
}