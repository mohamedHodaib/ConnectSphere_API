using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using MediatR;

namespace ConnectSphere.Application.Recommendations.Queries
{
    public sealed record GetUserSuggestionsQuery(
        Guid UserId,
        PaginationParameters PaginationParameters
    ) : IRequest<Result<PagedResult<PublicProfileInfoDto>>>;
}
