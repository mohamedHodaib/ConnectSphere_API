using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Models;
using ConnectSphere.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConnectSphere.Application.Notifications.Commands.MarkAllAsRead
{
    internal sealed class MarkAllNotificationsAsReadCommandHandler : IRequestHandler<MarkAllNotificationsAsReadCommand
        , Result<Unit>>
    {
        private readonly ILogger<MarkAllNotificationsAsReadCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MarkAllNotificationsAsReadCommandHandler(
            ILogger<MarkAllNotificationsAsReadCommandHandler> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(MarkAllNotificationsAsReadCommand request
            , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to mark all notifications as read for user {UserId}",
                request.UserId);
            
            //Mark the notification as read.
            _unitOfWork.NotificationRepository.MarkAllAsReadAsync(request.UserId);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("All Notifications marked as read successfully for user {UserId}", request.UserId);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}