using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace ConnectSphere.Application.Features.Profiles.Queries.GetPublicProfileInfo
{
    public record GetPublicProfileInfoQuery(string Username) : IRequest<Result<PublicProfileInfoDto>>;
}