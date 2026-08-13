using ConnectSphere.Application.Features.Bookmarks.Commands.DeleteBookmark;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Application.Features.Bookmarks.Queries.GetUserBookmarks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Features.Bookmarks.Queries.GetUserBookmarks
{
    internal sealed class GetUserBookmarksQueryHandler 
        : IRequestHandler<GetUserBookmarksQuery, Result<PagedResult<PostDetailsDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserBookmarksQueryHandler> _logger;

        public GetUserBookmarksQueryHandler(IUnitOfWork unitOfWork
            , ILogger<GetUserBookmarksQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<PagedResult<PostDetailsDto>>> Handle
            (GetUserBookmarksQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Proccess retrieving Bookmarks for user {userId}.", request.UserId);

            var pagedDtos = await _unitOfWork.BookmarkRepository.GetUserBookmarksAsync(request.UserId, request.PaginationDto);

            var pagedResult = new PagedResult<PostDetailsDto>
            (
                pagedDtos,
                pagedDtos.MetaData
            );

            _logger.LogInformation("Bookmarks is retrieved successfully for user {userId}.", request.UserId);

            return Result<PagedResult<PostDetailsDto>>.Success(pagedResult);
        }
    }
}