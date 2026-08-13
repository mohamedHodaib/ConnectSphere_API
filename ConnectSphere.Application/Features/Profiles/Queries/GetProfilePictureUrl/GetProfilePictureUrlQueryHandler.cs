using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Interfaces;
using ConnectSphere.Application.Profiles.Queries.GetProfilePictureUrl;
using MediatR;
using Microsoft.Extensions.Logging;

internal sealed class GetProfilePictureUrlQueryHandler
    : IRequestHandler<GetProfilePictureUrlQuery, Result<ProfilePictureUrlDto>>
{
    private readonly ILogger<GetProfilePictureUrlQueryHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUrlService _urlService;

    public GetProfilePictureUrlQueryHandler(
        ILogger<GetProfilePictureUrlQueryHandler> logger, IUnitOfWork unitOfWork, IUrlService urlService)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _urlService = urlService;
    }

    public async Task<Result<ProfilePictureUrlDto>> Handle(GetProfilePictureUrlQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting profile picture url for user {UserId}", request.UserId);

        var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId, trackChanges: false);
        if (user is null)
        {
            _logger.LogWarning("User {UserId} not found in database", request.UserId);
            return Result<ProfilePictureUrlDto>.Failure(Error.NotFound("User.NotFound", "User not found."));
        }

        var dto = new ProfilePictureUrlDto { ProfilePictureUrl = _urlService.GetAbsoluteUrl(user.ProfilePictureUrl) };

        return Result<ProfilePictureUrlDto>.Success(dto);
    }
}