using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Domain.Entities;
using ConnectSphere.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Repositories
{
    internal class TagRepository : RepositoryBase<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context)
          : base(context)
        {
        }


        public void CreateTagCollection(IEnumerable<Tag> tags) => CreateCollection(tags);

        public async Task<IEnumerable<Tag>> GetTagsByTagNamesAsync(IEnumerable<string> tagNames) =>
            await FindByCondition(t => tagNames.Contains(t.Name),true)
            .ToListAsync();  
    }
}
