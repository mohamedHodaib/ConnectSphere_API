using ConnectSphere.Application.Common.Models;
using MediatR;
using System;

namespace ConnectSphere.Application.Notifications.Commands.MarkAllAsRead
{
    public record MarkAllNotificationsAsReadCommand(Guid UserId) : IRequest<Result<Unit>>;
}