using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Notifications.Queries.GetUserNotifications
{
    public sealed record GetUserNotificationsQuery(Guid UserId,bool UnreadOnly, PaginationParameters PaginationDto)
    : IRequest<Result<PagedResult<NotificationDto>>>;

    
}
