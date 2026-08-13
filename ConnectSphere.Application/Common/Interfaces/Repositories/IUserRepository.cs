using AutoMapper.QueryableExtensions;
using ConnectSphere.Application.Common.DTOs;
using ConnectSphere.Application.Common.Pagination;
using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<bool> UserExistsAsync(Guid id);

        Task<User?> GetByIdAsync(Guid id, bool trackChanges = true);

        Task<PublicProfileInfoDto?> GetPublicProfileinfoByIdAsync(Guid id, bool trackChanges = true);

        Task<OwnProfileInfoDto?> GetOwnProfileinfoByIdAsync(Guid id, bool trackChanges = true);

        Task<OwnSettingsDto?> GetOwnSettingsByIdAsync(Guid id, bool trackChanges = true);
        Task<PublicProfileInfoDto?> GetPublicProfileInfoByUsernameAsync(string username, bool trackChanges = false);


        Task<PagedList<PublicProfileInfoDto>> GetSuggestionsAsync(Guid userId, IReadOnlySet<Guid> execludedUserIds, PaginationParameters paginationParameters);

        void DeleteUser(User user);
    }
}
