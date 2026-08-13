using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Models;
using FluentValidation;
using MediatR;

namespace ConnectSphere.Application.Profiles.Queries.GetOwnSettings
{
    public record GetOwnSettingsQuery(Guid UserId) : IRequest<Result<OwnSettingsDto>>;
}