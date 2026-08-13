using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Recommendations.Queries
{
    internal sealed class GetUserSuggestionsQueryHandler
        : IRequestHandler<GetUserSuggestionsQuery, Result<PagedResult<PublicProfileInfoDto>>>
    {
        private readonly ILogger<GetUserSuggestionsQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleService _roleService;

        public GetUserSuggestionsQueryHandler(
            ILogger<GetUserSuggestionsQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IRoleService roleService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _roleService = roleService;
        }

        public async Task<Result<PagedResult<PublicProfileInfoDto>>> Handle(
            GetUserSuggestionsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing follow suggestions for user {UserId}", request.UserId);

            //get admin Ids
            var adminIds = await _roleService.GetUserIdsInRoleAsync("Admin");

            var pagedDtos = await _unitOfWork.UserRepository.GetSuggestionsAsync(
                request.UserId,
                adminIds,
                request.PaginationParameters);

            var pagedResult = new PagedResult<PublicProfileInfoDto>(
                pagedDtos,
                pagedDtos.MetaData);

            _logger.LogInformation("Successfully retrieved {Count} follow suggestions for user {UserId}",
                pagedDtos.Count, request.UserId);

            return Result<PagedResult<PublicProfileInfoDto>>.Success(pagedResult);
        }
    }
}
