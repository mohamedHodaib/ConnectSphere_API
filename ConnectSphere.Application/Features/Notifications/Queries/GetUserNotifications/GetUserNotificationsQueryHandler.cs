using AutoMapper;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Application.Feed.Queries.GetFeed;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Features.Notifications.Queries.GetUserNotifications
{
    public class GetUserNotificationsQueryHandler
        : IRequestHandler<GetUserNotificationsQuery, Result<PagedResult<NotificationDto>>>
    {
        private readonly ILogger<GetUserNotificationsQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserNotificationsQueryHandler(
            ILogger<GetUserNotificationsQueryHandler> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<NotificationDto>>> Handle(GetUserNotificationsQuery request
            , CancellationToken ct)
        {
            _logger.LogInformation("Proccess retrieving Notifications for user {userId}.", request.UserId);

            var pagedDtos = await _unitOfWork.NotificationRepository
                .GetUserNotificationsAsync(request.UserId, request.UnreadOnly, request.PaginationDto);

            var pagedResult = new PagedResult<NotificationDto>
            (
                pagedDtos,
                pagedDtos.MetaData
            );

            _logger.LogInformation("Notifications is retrieved successfully for user {userId}.", request.UserId);

            return Result<PagedResult<NotificationDto>>.Success(pagedResult);
        }
    }
}
