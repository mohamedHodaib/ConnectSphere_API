using ConnectSphere.Application.Common.Interfaces; // Assuming interface for current user
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Follows.Commands.Follows;
using ConnectSphere.Domain.Entities; // Ensure Domain entities are imported
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Follows.Commands.Unfollow
{
    // Fix: PascalCase naming
    internal sealed class UnfollowCommandHandler : IRequestHandler<UnfollowCommand, Result<Unit>>
    {
        private readonly ILogger<UnfollowCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnfollowCommandHandler(
            ILogger<UnfollowCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(UnfollowCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("Attempting to delete follow relationship: {FollowerId} -> {FollowingId}",
                request.FollowerId, request.FollowingId);

            //checking that follower id and following id are different in fluent validation.

            //checking the relationship existence. 
            var follow = await _unitOfWork.FollowRepository
                .GetFollow(request.FollowerId, request.FollowingId);

            if (follow is null)
            {
                _logger.LogWarning("User {followerId} is not following user {followingId}",
                    request.FollowerId, request.FollowingId);

                return Result<Unit>.Failure(Error.NotFound("Follow.NotFound", "You are not following this user."));
            }

            // Delete following
            _unitOfWork.FollowRepository.DeleteFollow(follow);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Successfully deleted follow relationship: {FollowerId} -> {FollowingId}",
                request.FollowerId, request.FollowingId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}