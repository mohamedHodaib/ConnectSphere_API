using AutoMapper;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Reactions.Commands.AddReaction;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Reactions.Commands.RemoveReaction
{
    internal sealed class RemoveReactionCommandHandler : IRequestHandler<RemoveReactionCommand, Result<Unit>>
    {
        private readonly ILogger<RemoveReactionCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveReactionCommandHandler(
            ILogger<RemoveReactionCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(RemoveReactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing removing reaction for post {PostId} by user {UserId}"
                , request.PostId, request.UserId);

            //Validate Post Existence (We don't use the method that returning the whole post object because we don't need to
            //know any thing about the post we only want to know If it exists or not).
            var postExists = await _unitOfWork.PostRepository.PostExistsAsync(request.PostId);
            if (!postExists)
            {
                _logger.LogWarning("User {UserId} attempted to react to non-existent post {PostId}"
                    , request.UserId, request.PostId);
                return Result<Unit>.Failure(Error.NotFound("Post.NotFound", "Post not found."));
            }

            //Checking Reaction Existence
            var existingReaction = await _unitOfWork.ReactionRepository
                .GetReactionByPostIdAndUserIdAsync(request.PostId, request.UserId);

            if (existingReaction == null)
            {
                _logger.LogInformation("User {userId} does not have a reaction on post {postId}."
                    , request.UserId, request.PostId);

                return Result<Unit>.Failure(Error.NotFound("Reaction.NotFound"
                    , $"you don't have a reaction on post {request.PostId}."));
            }
            
            //Remove the reaction
            _unitOfWork.ReactionRepository.DeleteReaction(existingReaction);
            await _unitOfWork.CompleteAsync();

            return Result<Unit>.Success(Unit.Value);
        }

    }
}
