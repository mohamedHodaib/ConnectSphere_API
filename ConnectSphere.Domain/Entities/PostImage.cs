using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Domain.Entities
{
    public class PostImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public Guid PostId { get; set; }

        //RelationShips
        public Post Post { get; set; } = null!;
    }
}
