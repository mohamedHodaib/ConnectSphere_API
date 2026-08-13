using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<User> _userManager;

        public RoleService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<HashSet<Guid>> GetUserIdsInRoleAsync(string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);

            return users
                .Select(u => u.Id)
                .ToHashSet();
        }
    }
}
