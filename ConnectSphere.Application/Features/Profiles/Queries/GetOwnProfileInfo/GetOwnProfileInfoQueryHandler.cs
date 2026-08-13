using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Interfaces;
using ConnectSphere.Application.Posts.Commands.UpdatePost;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Profiles.Queries.GetOwnProfileInfo
{
    internal sealed class GetOwnProfileInfoQueryHandler
        : IRequestHandler<GetOwnProfileInfoQuery, Result<OwnProfileInfoDto>>
    {
        private readonly ILogger<GetOwnProfileInfoQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUrlService _urlService;

        public GetOwnProfileInfoQueryHandler(
            ILogger<GetOwnProfileInfoQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUrlService urlService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _urlService = urlService;
        }

        public async Task<Result<OwnProfileInfoDto>> Handle
            (GetOwnProfileInfoQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting own profile info for user {userId}", request.UserId);

            //We project the user entity to OwnProfileInfoDto in the User repository to prevent passwordhash to be returned
            var ownProfileInfo = await _unitOfWork.UserRepository.GetOwnProfileinfoByIdAsync(request.UserId,false);

            ownProfileInfo.ProfilePictureUrl = _urlService.GetAbsoluteUrl(ownProfileInfo.ProfilePictureUrl);

            //Ensure user actually exists before mapping
            if (ownProfileInfo is null)
            {
                _logger.LogWarning("User {userId} not found in database", request.UserId);
                return Result<OwnProfileInfoDto>.Failure(Error.NotFound("User.NotFound", "User profile not found."));
            }

            _logger.LogInformation("User {userId} optained successfully.", request.UserId);

            return Result<OwnProfileInfoDto>.Success(ownProfileInfo);
        }
    }
}
