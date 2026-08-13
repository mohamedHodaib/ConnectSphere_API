using ConnectSphere.Application.Common.Interfaces.Repositories;
using ConnectSphere.Application.Common.Interfaces.Services;
using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Tag>> PrepareTagsAsync(List<string>? rawTags, CancellationToken ct)
        {
            if (rawTags == null || rawTags.Count == 0) return new List<Tag>();

            var distinctTags = rawTags
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => t.Trim().ToLowerInvariant())
                .Distinct()
                .ToHashSet();

            var existingTags = await _unitOfWork.TagRepository.GetTagsByTagNamesAsync(distinctTags);
            var existingTagNames = existingTags.Select(t => t.Name).ToHashSet();

            var newTags = distinctTags
                .Where(name => !existingTagNames.Contains(name))
                .Select(Tag.Create)
                .ToList();

            if (newTags.Any())
            {
                _unitOfWork.TagRepository.CreateTagCollection(newTags);
            }

            return existingTags.Concat(newTags).ToList();
        }
    }

}
