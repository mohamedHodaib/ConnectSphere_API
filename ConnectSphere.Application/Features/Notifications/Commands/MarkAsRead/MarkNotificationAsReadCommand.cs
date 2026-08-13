using ConnectSphere.Application.Common.Models;
using MediatR;
using System;

namespace ConnectSphere.Application.Notifications.Commands.MarkAsRead
{
    public record MarkNotificationAsReadCommand(Guid NotificationId,Guid UserId) : IRequest<Result<Unit>>;
}