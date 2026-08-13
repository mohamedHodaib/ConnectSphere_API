using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace ConnectSphere.Application.Profiles.Queries.GetProfilePictureUrl
{
    public record GetProfilePictureUrlQuery(Guid UserId) : IRequest<Result<ProfilePictureUrlDto>>;

    
}