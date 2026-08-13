using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Services
{
    public interface IRoleService
    {
        Task<HashSet<Guid>> GetUserIdsInRoleAsync(string role);
    }
}
