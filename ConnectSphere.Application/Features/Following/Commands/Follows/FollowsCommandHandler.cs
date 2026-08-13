using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models; 
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Follows.Commands.Follows
{
    internal sealed class FollowsCommandHandler : IRequestHandler<FollowsCommand, Result<Unit>>
    {
        private readonly ILogger<FollowsCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public FollowsCommandHandler(
            ILogger<FollowsCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(FollowsCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create follow relationship: {FollowerId} -> {FollowingId}",
                request.FollowerId, request.FollowingId);

            // Check if the following will be done between different users
            if (request.FollowerId == request.FollowingId)
            {
                _logger.LogWarning("User {FollowerId} attempted to follow themselves.", request.FollowerId);

                return Result<Unit>.Failure(Error.Validation("Follow.SelfFollow", "You cannot follow yourself."));
            }

            //  Check if the target user exists
            var targetUserExists = await _unitOfWork.UserRepository.UserExistsAsync(request.FollowingId);

            if (!targetUserExists)
            {
                _logger.LogWarning("Target user {FollowingId} not found for follow request.", request.FollowingId);

                return Result<Unit>.Failure(Error.NotFound("User.NotFound"
                    , "The user you are trying to follow does not exist."));
            }

            // Check for existing relationship (Idempotency)
            var followExists = await _unitOfWork.FollowRepository.FollowExistsAsync(request.FollowerId, request.FollowingId);

            if (followExists)
            {
                // but returning Conflict informs the client the action was unnecessary.
                return Result<Unit>.Failure(Error.Conflict("Follow.Duplicate", "You are already following this user."));
            }

            // Create Domain Entity
            var newFollow = Follow.Create(request.FollowerId, request.FollowingId);

            _unitOfWork.FollowRepository.CreateFollow(newFollow);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Successfully created follow relationship: {FollowerId} -> {FollowingId}",
                request.FollowerId, request.FollowingId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}