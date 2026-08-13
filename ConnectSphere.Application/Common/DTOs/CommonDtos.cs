using ConnectSphere.Application.Common.Pagination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.DTOs
{
    public record class PaginationParameters
    {
        private const int MaxPageSize = 50;

        // Default value is 1. Attribute validates the input from the client.
        public int PageNumber { get; init; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            // The acts as a final safeguard that caps the value.
            init => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

    public record PagedResult<T>(IEnumerable<T> Items, MetaData MetaData);
}
