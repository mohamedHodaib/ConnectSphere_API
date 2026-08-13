using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Posts.Commands.CreatePost;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Posts.Queries.GetPost
{
    internal sealed class GetPostQueryHandler : IRequestHandler<GetPostQuery, Result<PostDetailsDto>>
    {
        private readonly ILogger<GetPostQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPostQueryHandler(
            ILogger<GetPostQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PostDetailsDto>> Handle(GetPostQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Getting post with Id:{postId}.", request.Id);

            var post = await _unitOfWork.PostRepository.GetPostByIdIncludeUserAndImagesAsync(request.Id,false);

            if (post == null)
            {
                _logger.LogWarning("Post with Id:{postId} was not found.", request.Id);

                return Result<PostDetailsDto>.Failure(Error.NotFound("Post.NotFound", $"Post with id {request.Id} not found"));
            }

            var PostDetailsDto = _mapper.Map<PostDetailsDto>(post);

            _logger.LogInformation("Post with Id:{postId} optained successfully.", request.Id);

            return Result<PostDetailsDto>.Success(PostDetailsDto);
        }
    }
}
