using AutoMapper;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Posts.Commands.DeletePost;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Posts.Commands.DeletePost
{
    internal sealed class DeletePostCommandHandler : IRequestHandler<DeletePostCommand,Result<Unit>>
    {
        private readonly ILogger<DeletePostCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public DeletePostCommandHandler(
            ILogger<DeletePostCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITagService tagService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tagService = tagService;
        }

        public async Task<Result<Unit>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to deleting post {PostId} by user {UserId}",
               request.PostId, request.UserId);

            //Checking existence for the post.
            var existingPost = await _unitOfWork.PostRepository
                .GetPostByIdAsync(request.PostId);

            if (existingPost is null)
            {
                _logger.LogWarning("Delete failed. Post {PostId} not found", request.PostId);
                return Result<Unit>.Failure(Error.NotFound("Post.NotFound", $"Post with id {request.PostId} not found"));
            }

            // Delete only if he is the owner of the post or the admin
            if (!request.IsAdmin && existingPost.UserId != request.UserId)
            {
                _logger.LogWarning("Authorization failed. User {UserId} who isn't the admin attempted" +
                    " to delete post {PostId} owned by {OwnerId}",
                    request.UserId, request.PostId, existingPost.UserId);

                return Result<Unit>.Failure(Error.Forbidden("Post.Forbidden", "You do not have permission to delete this post."));
            }

            //Delete the post.
            _unitOfWork.PostRepository.DeletePost(existingPost);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Post {PostId} deleted successfully", request.PostId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
