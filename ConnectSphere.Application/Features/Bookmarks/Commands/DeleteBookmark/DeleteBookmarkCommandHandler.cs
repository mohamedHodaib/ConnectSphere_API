using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Features.Bookmarks.Commands.DeleteBookmark
{
    internal sealed class DeleteBookmarkCommandHandler : IRequestHandler<DeleteBookmarkCommand, Result<Unit>>
    {
        private readonly ILogger<DeleteBookmarkCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookmarkCommandHandler(ILogger<DeleteBookmarkCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(DeleteBookmarkCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to remove bookmark for post {PostId} by user {UserId}", request.PostId, request.UserId);

            var bookmark = await _unitOfWork.BookmarkRepository.GetBookmark(request.UserId, request.PostId, trackChanges: true);
            if (bookmark is null)
            {
                _logger.LogWarning("Remove bookmark failed. No bookmark found for post {PostId} by user {UserId}", request.PostId, request.UserId);
                return Result<Unit>.Failure(Error.NotFound("Bookmark.NotFound", "You have not bookmarked this post."));
            }

            _unitOfWork.BookmarkRepository.DeleteBookmark(bookmark);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Bookmark removed successfully for post {PostId} by user {UserId}", request.PostId, request.UserId);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}