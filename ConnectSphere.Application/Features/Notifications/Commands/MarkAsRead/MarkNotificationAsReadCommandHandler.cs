using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Notifications.Commands.MarkAsRead
{
    internal sealed class MarkNotificationAsReadCommandHandler : IRequestHandler<MarkNotificationAsReadCommand, Result<Unit>>
    {
        private readonly ILogger<MarkNotificationAsReadCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MarkNotificationAsReadCommandHandler(
            ILogger<MarkNotificationAsReadCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to mark notification {NotificationId} as read by user {UserId}",
                request.NotificationId, request.UserId);

            //Checking existence for the notification, scoped to the requesting user.
            var existingNotification = await _unitOfWork.NotificationRepository
                .GetNotificationAsync(request.NotificationId, request.UserId, trackChanges: true);

            if (existingNotification is null)
            {
                _logger.LogWarning("Mark as read failed. Notification {NotificationId} not found for user {UserId}",
                    request.NotificationId, request.UserId);
                return Result<Unit>.Failure(Error.NotFound("Notification.NotFound", $"Notification with id {request.NotificationId} not found"));
            }
            
            //Mark the notification as read.
            existingNotification.MarkAsRead();
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Notification {NotificationId} marked as read successfully", request.NotificationId);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}