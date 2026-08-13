using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Comments.Queries.ViewComments
{
    internal sealed class ViewCommentsQueryHandler : IRequestHandler<ViewCommentsQuery, Result<PagedResult<CommentDetailsDto>>>
    {
        private readonly ILogger<ViewCommentsQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ViewCommentsQueryHandler(
            ILogger<ViewCommentsQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<CommentDetailsDto>>> Handle(ViewCommentsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Process retrieving comments for post {postId}",request.PostId);
            //check existence of the post 
            //Validate Post Existence (We don't use the method that returning the whole post object because we don't need to
            //know any thing about the post we only want to know If it exists or not).
            var postExists = await _unitOfWork.PostRepository.PostExistsAsync(request.PostId);
            if (!postExists)
            {
                _logger.LogWarning("Cannot retrieve comments to non-existent post {PostId}", request.PostId);

                return Result<PagedResult<CommentDetailsDto>>.Failure(Error.NotFound("Post.NotFound", "Post not found."));
            }

            var pagedDtos = await _unitOfWork.CommentRepository.GetCommentsAsync(request.PostId, request.PaginationDto);

            var pagedResult = new PagedResult<CommentDetailsDto>
            (
                pagedDtos,
                pagedDtos.MetaData
            );


            _logger.LogInformation("Comments retrieved successfully for post {postId}",request.PostId);

            return Result<PagedResult<CommentDetailsDto>>.Success(pagedResult);
        }
    }
}