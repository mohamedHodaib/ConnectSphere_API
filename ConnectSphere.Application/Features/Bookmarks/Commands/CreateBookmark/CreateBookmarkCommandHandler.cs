using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Features.Bookmarks.Commands.CreateBookmark
{
    internal sealed class CreateBookmarkCommandHandler : IRequestHandler<CreateBookmarkCommand, Result<Unit>>
    {
        private readonly ILogger<CreateBookmarkCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBookmarkCommandHandler(ILogger<CreateBookmarkCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(CreateBookmarkCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to bookmark post {PostId} by user {UserId}", request.PostId, request.UserId);

            //Checking the post actually exists.
            var postExists = await _unitOfWork.PostRepository.PostExistsAsync(request.PostId);
            if (!postExists)
            {
                _logger.LogWarning("Bookmark failed. Post {PostId} not found", request.PostId);
                return Result<Unit>.Failure(Error.NotFound("Post.NotFound", $"Post with id {request.PostId} not found"));
            }

            //Checking for an existing bookmark to avoid duplicates.
            var alreadyBookmarked = await _unitOfWork.BookmarkRepository.BookmarkExistsAsync(request.UserId, request.PostId);
            if (alreadyBookmarked)
            {
                _logger.LogWarning("Bookmark failed. Post {PostId} already bookmarked by user {UserId}", request.PostId, request.UserId);
                return Result<Unit>.Failure(Error.Conflict("Bookmark.AlreadyExists", "You have already bookmarked this post."));
            }

            var bookmark = Bookmark.Create(request.UserId, request.PostId);
            _unitOfWork.BookmarkRepository.CreateBookmark(bookmark);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post {PostId} bookmarked successfully by user {UserId}", request.PostId, request.UserId);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}