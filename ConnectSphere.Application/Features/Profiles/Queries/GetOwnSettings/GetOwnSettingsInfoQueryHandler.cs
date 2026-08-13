using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Profiles.Queries.GetOwnSettings
{
    internal sealed class GetOwnSettingsQueryHandler
        : IRequestHandler<GetOwnSettingsQuery, Result<OwnSettingsDto>>
    {
        private readonly ILogger<GetOwnSettingsQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrlService _urlService;

        public GetOwnSettingsQueryHandler(
            ILogger<GetOwnSettingsQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IUrlService urlService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _urlService = urlService;
        }

        public async Task<Result<OwnSettingsDto>> Handle
            (GetOwnSettingsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start getting own settings for user {userId}", request.UserId);

            //We project the user entity to OwnSettingsDto in the User repository to prevent passwordhash to be returned
            var ownSettings = await _unitOfWork.UserRepository.GetOwnSettingsByIdAsync(request.UserId, false);

            //Ensure user actually exists before touching its properties
            if (ownSettings is null)
            {
                _logger.LogWarning("User {userId} not found in database", request.UserId);
                return Result<OwnSettingsDto>.Failure(Error.NotFound("User.NotFound", "User settings not found."));
            }

            ownSettings.ProfilePictureUrl = _urlService.GetAbsoluteUrl(ownSettings.ProfilePictureUrl);

            _logger.LogInformation("Settings for user {userId} obtained successfully.", request.UserId);
            return Result<OwnSettingsDto>.Success(ownSettings);
        }
    }
}