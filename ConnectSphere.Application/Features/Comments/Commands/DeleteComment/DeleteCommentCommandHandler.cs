using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Comments.Commands.DeleteComment
{
    internal sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result<Unit>>
    {
        private readonly ILogger<DeleteCommentCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCommentCommandHandler(
            ILogger<DeleteCommentCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
            
       

        public async Task<Result<Unit>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {userId} attempt to deleting comment {commentId} on post {postId}"
                , request.userId,request.CommentId,request.PostId);

            //check existence of the post 
            //Validate Post Existence (We don't use the method that returning the whole post object because we don't need to
            //know any thing about the post we only want to know If it exists or not).
            var postExists = await _unitOfWork.PostRepository.PostExistsAsync(request.PostId);
            if (!postExists)
            {
                _logger.LogWarning("User {UserId} attempted to delete comment on non-existent post {PostId}"
                    , request.userId, request.PostId);

                return Result<Unit>.Failure(Error.NotFound("Post.NotFound", "Post not found."));
            }

            var comment = await _unitOfWork.CommentRepository.GetCommentByIdAsync(request.CommentId);

            if (comment is null)
            {
                _logger.LogWarning("User {UserId} attempted to delete non-existent comment on  post {PostId}"
                    , request.userId, request.PostId);

                return Result<Unit>.Failure(Error.NotFound("Comment.NotFound", "Comment not found."));
            }

            //Check if the user is not the author of the comment or the post
            if(comment.CommentedUserId != request.userId 
                && comment.Post.UserId != request.userId)
            {
                _logger.LogWarning("User {UserId} not own the post {postId} or attempted to delete another user's comment."
                    , request.userId, request.PostId);

                return Result<Unit>.Failure(Error.Conflict("Comment.Conflict"
                    , "Comment not owned by the user or the user isn't the author of the post."));
            }

            //delete the comment
            _unitOfWork.CommentRepository.DeleteComment(comment);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Comment {commentId} deleted successfully by user {userId} on post {postId}"
                ,request.CommentId, request.userId,request.PostId);

            return Result<Unit>.Success(Unit.Value);
        }
    }
}