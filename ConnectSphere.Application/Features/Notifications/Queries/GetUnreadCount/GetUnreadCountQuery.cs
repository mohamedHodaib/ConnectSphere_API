using MediatR;
using System;

namespace ConnectSphere.Application.Notifications.Queries.GetUnreadCount
{
    public record GetUnreadNotificationsCountQuery(Guid UserId) : IRequest<int>;
}