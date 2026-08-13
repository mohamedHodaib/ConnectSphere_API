using ConnectSphere.Application.Common.Models;
using MediatR;

namespace ConnectSphere.Application.Features.Auth.Commands.ConfirmEmail
{
    public sealed record ConfirmEmailCommand(string UserId, string Token) : IRequest<Result>;
}
