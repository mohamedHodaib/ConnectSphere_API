using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface ITagRepository : IRepositoryBase<Tag>
    {
         void CreateTagCollection(IEnumerable<Tag> tags);
         Task<IEnumerable<Tag>> GetTagsByTagNamesAsync(IEnumerable<string> tagNames);
    }
}
