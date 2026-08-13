using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Application.Posts.Queries.GetPost;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Feed.Queries.GetFeed
{
    internal sealed class GetFeedQueryHandler 
        : IRequestHandler<GetFeedQuery,Result<PagedResult<PostDetailsDto>>>
    {
        private readonly ILogger<GetFeedQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetFeedQueryHandler(
            ILogger<GetFeedQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<PostDetailsDto>>> Handle(GetFeedQuery request
            , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Proccess retrieving feed for user {userId}.", request.UserId);

            var pagedDtos = await _unitOfWork.PostRepository.GetFeedAsync(request.UserId, request.PaginationDto);

            var pagedResult = new PagedResult<PostDetailsDto>
            (
                pagedDtos,
                pagedDtos.MetaData
            );

            _logger.LogInformation("Feed is retrieved successfully for user {userId}.", request.UserId);

            return Result<PagedResult<PostDetailsDto>>.Success(pagedResult);
        }
    }
}
