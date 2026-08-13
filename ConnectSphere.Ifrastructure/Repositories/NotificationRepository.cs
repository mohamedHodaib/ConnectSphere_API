using AutoMapper;
using AutoMapper.QueryableExtensions;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Domain.Enums;
using ConnectSphere.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ConnectSphere.Infrastructure.Persistence.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {

        private readonly IMapper _mapper;

        public NotificationRepository(ApplicationDbContext context, IMapper mapper)
          : base(context)
        {
            _mapper = mapper;
        }
        public void CreateNotification(Notification notification) => Create(notification);

        public void DeleteNotification(Notification notification) => Delete(notification);

        public async Task<Notification?> GetNotificationAsync(Guid notificationId, Guid userId, bool trackChanges = true)
            => await FindByCondition(n => n.Id == notificationId && n.UserId == userId, trackChanges)
                .SingleOrDefaultAsync();

        public async Task<PagedList<NotificationDto>> GetUserNotificationsAsync(
            Guid userId, bool unreadOnly,PaginationParameters paginationParameters, bool trackChanges = false)
        {
            IQueryable<Notification> query = FindByCondition(
                n => n.UserId == userId
                , false)
                .OrderByDescending(p => p.CreatedAt);

            if (unreadOnly) query = query.Where(n => !n.IsRead);

            var projectedQuery = query.ProjectTo<NotificationDto>(_mapper.ConfigurationProvider);

            return await PagedList<NotificationDto>
                 .ToPagedList(projectedQuery, paginationParameters.PageNumber, paginationParameters.PageSize);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
            => await FindByCondition(n => n.UserId == userId && !n.IsRead, trackChanges: false)
                .CountAsync();
        public async Task MarkAllAsReadAsync(Guid userId)
            => await FindByCondition(n => n.UserId == userId && !n.IsRead, trackChanges: false)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));
    }
}