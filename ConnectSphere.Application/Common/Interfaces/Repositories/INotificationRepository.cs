using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        void CreateNotification(Notification notification);
        void DeleteNotification(Notification notification);
        Task<Notification?> GetNotificationAsync(Guid notificationId, Guid userId, bool trackChanges = true);
        Task<PagedList<NotificationDto>> GetUserNotificationsAsync(
            Guid userId, bool unreadOnly,PaginationParameters paginationParameters, bool trackChanges = false);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task MarkAllAsReadAsync(Guid userId);
    }
}