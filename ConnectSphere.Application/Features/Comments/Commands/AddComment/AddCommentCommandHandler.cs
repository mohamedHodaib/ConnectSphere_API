using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Comments.Commands.AddComment
{
    internal sealed class AddCommentsCommandHandler : IRequestHandler<AddCommentCommand, Result<CommentDetailsDto>>
    {
        private readonly ILogger<AddCommentsCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddCommentsCommandHandler(
            ILogger<AddCommentsCommandHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CommentDetailsDto>> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User {userId} start creating comment on post {postId}"
                , request.CommentedUserId,request.AddCommentDto.PostId);

            //check existence of the post 
            //Validate Post Existence (We don't use the method that returning the whole post object because we don't need to
            //know any thing about the post we only want to know If it exists or not).
            var postExists = await _unitOfWork.PostRepository.PostExistsAsync(request.AddCommentDto.PostId);
            if (!postExists)
            {
                _logger.LogWarning("User {UserId} attempted to comment to non-existent post {PostId}"
                    , request.CommentedUserId, request.AddCommentDto.PostId);

                return Result<CommentDetailsDto>.Failure(Error.NotFound("Post.NotFound", "Post not found."));
            }
            //Create The Comment
            var newComment = Comment.Create(
                request.CommentedUserId,
                request.PostId,
                request.AddCommentDto.Content
            );


            _unitOfWork.CommentRepository.CreateComment( newComment );
            await _unitOfWork.CompleteAsync();

            var commentDetailsDto = _mapper.Map<CommentDetailsDto>(newComment);

            _logger.LogInformation("Comment created successfully by user {userId} on post {postId}"
                , request.CommentedUserId,request.AddCommentDto.PostId);

            return Result<CommentDetailsDto>.Success(commentDetailsDto);
        }
    }
}