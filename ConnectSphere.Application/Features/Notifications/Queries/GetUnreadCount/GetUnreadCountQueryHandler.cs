using ConnectSphere.Application.Common.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Notifications.Queries.GetUnreadCount
{
    internal sealed class GetUnreadNotificationsCountQueryHandler
        : IRequestHandler<GetUnreadNotificationsCountQuery, int>
    {
        private readonly ILogger<GetUnreadNotificationsCountQueryHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public GetUnreadNotificationsCountQueryHandler(
            ILogger<GetUnreadNotificationsCountQueryHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetUnreadNotificationsCountQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting unread notifications count for user {UserId}", request.UserId);

            var count = await _unitOfWork.NotificationRepository.GetUnreadCountAsync(request.UserId);

            _logger.LogInformation("User {UserId} has {Count} unread notifications", request.UserId, count);

            return count;
        }
    }
}