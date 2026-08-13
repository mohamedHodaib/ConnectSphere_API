using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Posts.Commands.UpdatePost
{
    internal sealed class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result<Unit>>
    {
        private readonly ILogger<UpdatePostCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagService _tagService;

        public UpdatePostCommandHandler(
            ILogger<UpdatePostCommandHandler> logger,
            IUnitOfWork unitOfWork,
            ITagService tagService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _tagService = tagService;
        }

        public async Task<Result<Unit>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update post {PostId} by user {UserId}",
                request.PostId, request.UserId);

            //Checking existence for the post.
            var existingPost = await _unitOfWork.PostRepository
                .GetPostByIdIncludeTagsAsync(request.PostId);

            if (existingPost is null)
            {
                _logger.LogWarning("Update failed. Post {PostId} not found", request.PostId);
                return Result<Unit>.Failure(Error.NotFound("Post.NotFound", $"Post with id {request.PostId} not found"));
            }

            // "They should not be able to edit posts by other users."
            if (existingPost.UserId != request.UserId)
            {
                _logger.LogWarning("Authorization failed. User {UserId} attempted to update post {PostId} owned by {OwnerId}",
                    request.UserId, request.PostId, existingPost.UserId);

                return Result<Unit>.Failure(Error.Forbidden("Post.Forbidden", "You do not have permission to update this post."));
            }

            //Handle Tags 
            var tags = await _tagService.PrepareTagsAsync(request.UpdatePostDto.Tags, cancellationToken);

            //Update the post
            existingPost.Update(request.UpdatePostDto.Content, tags);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post {PostId} updated successfully", request.PostId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}