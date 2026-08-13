using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Profiles.Queries.GetPublicProfileInfo
{
    internal sealed class GetPublicProfileInfoQueryHandler
        : IRequestHandler<GetPublicProfileInfoQuery, Result<PublicProfileInfoDto>>
    {
        private readonly ILogger<GetPublicProfileInfoQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlService _urlService;

        public GetPublicProfileInfoQueryHandler(
            ILogger<GetPublicProfileInfoQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IUrlService urlService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _urlService = urlService;
        }

        public async Task<Result<PublicProfileInfoDto>> Handle
            (GetPublicProfileInfoQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting public profile info for username {username}", request.Username);

            var profileInfo = await _unitOfWork.UserRepository.GetPublicProfileInfoByUsernameAsync(request.Username, false);

            if (profileInfo is null)
            {
                _logger.LogWarning("User with username {username} not found in database", request.Username);
                return Result<PublicProfileInfoDto>.Failure(Error.NotFound("User.NotFound", "User profile not found."));
            }


            profileInfo.ProfilePictureUrl = _urlService.GetAbsoluteUrl(profileInfo.ProfilePictureUrl);

            _logger.LogInformation("Public profile info for username {username} obtained successfully.", request.Username);
            return Result<PublicProfileInfoDto>.Success(profileInfo);
        }
    }
}