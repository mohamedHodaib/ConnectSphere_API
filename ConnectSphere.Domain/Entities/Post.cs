using ConnectSphere.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //RelationShips
        public User User { get; set; } = null!;
        public ICollection<PostImage> Images { get; set; } = new List<PostImage>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        // We make it private so developers can't use "new Post()" incorrectly.
        private Post() { }

        public static Post Create(Guid userId, string content)
        {
            // Create the instance
            var post = new Post
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow, 
                UpdatedAt = null
            };

            return post;
        }


        public void Update(string newContent,List<Tag> tags)
        {

            this.Content = newContent;
            this.Tags = tags;
            this.UpdatedAt = DateTime.UtcNow;
        }

    }
}
