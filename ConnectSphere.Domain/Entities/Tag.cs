using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Domain.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;

        //For RelationShips
        public ICollection<Post> Posts { get; set; } = new List<Post>();


        private Tag() { }

        public static Tag Create(string name)
        {
            return new Tag()
            {
                Id = Guid.NewGuid(),
                Name = name
            };
        }
            
    }
}
