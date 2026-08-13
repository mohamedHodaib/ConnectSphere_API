using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Feed.Queries.GetFeed
{
    public sealed record GetFeedQuery(Guid UserId,PaginationParameters PaginationDto) 
        : IRequest<Result<PagedResult<PostDetailsDto>>>;
}
