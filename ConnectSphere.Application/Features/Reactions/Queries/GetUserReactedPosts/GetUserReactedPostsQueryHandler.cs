using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Reactions.Queries.GetUserReactedPosts
{
    internal sealed class GetUserReactedPostsQueryHandler
        : IRequestHandler<GetUserReactedPostsQuery, Result<PagedResult<PostDetailsDto>>>
    {
        private readonly ILogger<GetUserReactedPostsQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserReactedPostsQueryHandler(ILogger<GetUserReactedPostsQueryHandler> logger
            , IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<PagedResult<PostDetailsDto>>> Handle(GetUserReactedPostsQuery request
            , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting liked posts for user {UserId}", request.UserId);

            var likedPosts = await _unitOfWork.ReactionRepository
                .GetUserReactedPostsAsync(request.UserId, request.PaginationDto);

            var pagedResult = new PagedResult<PostDetailsDto>
            (
                likedPosts,
                likedPosts.MetaData
            );

            _logger.LogInformation("Liked posts retrieved successfully for user {UserId}", request.UserId);

            return Result<PagedResult<PostDetailsDto>>.Success(pagedResult);
        }
    }
}