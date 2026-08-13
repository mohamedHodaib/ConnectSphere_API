using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Services
{
    public interface ITagService
    {
        Task<List<Tag>> PrepareTagsAsync(List<string>? rawTags, CancellationToken ct);
    }
}
