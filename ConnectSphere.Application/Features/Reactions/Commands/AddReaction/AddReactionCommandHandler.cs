using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Reactions.Commands.AddReaction
{
    internal sealed class AddReactionCommandHandler : IRequestHandler<AddReactionCommand, Result<Unit>>
    {
        private readonly ILogger<AddReactionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        // private readonly ICacheService _cacheService; 

        public AddReactionCommandHandler(
            ILogger<AddReactionCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(AddReactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing reaction for post {PostId} by user {UserId}", request.PostId, request.UserId);

            //Validate Post Existence (We don't use the method that returning the whole post object because we don't need to
            //know any thing about the post we only want to know If it exists or not).
            var postExists = await _unitOfWork.PostRepository.PostExistsAsync(request.PostId);
            if (!postExists)
            {
                _logger.LogWarning("User {UserId} attempted to react to non-existent post {PostId}", request.UserId, request.PostId);
                return Result<Unit>.Failure(Error.NotFound("Post.NotFound","Post not found.")); 
            }

            //Retrieve Existing Reaction
            var existingReaction = await _unitOfWork.ReactionRepository
                .GetReactionByPostIdAndUserIdAsync(request.PostId, request.UserId);

            if (existingReaction == null)
            {
                // Create Logic (Add)
                var newReaction = Reaction.Create(request.UserId, request.PostId, request.CreateReactionDto.Type);
                _unitOfWork.ReactionRepository.CreateReaction(newReaction);

                _logger.LogInformation("Reaction created for post {PostId} by user {UserId}", request.PostId, request.UserId);
            }
            else
            {
                    // Update Logic
                    existingReaction.Update(request.CreateReactionDto.Type);
                    _logger.LogInformation("Reaction updated for post {PostId} by user {UserId}", request.PostId, request.UserId);
            }

            await _unitOfWork.CompleteAsync();

            return Result<Unit>.Success(Unit.Value);
        }
    }
}