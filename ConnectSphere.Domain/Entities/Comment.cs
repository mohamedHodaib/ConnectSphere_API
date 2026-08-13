using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid CommentedUserId { get; set; }
        public string Content { get; set; } = null!;

        //parent comment Id that this comment replies to it.
        public Guid? ParentCommentId { get; set; }
        public DateTime CreatedAt { get; set; }

        //For RelationShips
        public User User { get; set; } = null!;
        public Post Post { get; set; } = null!;
        public Comment ParentComment { get; set; } = null!;
        public ICollection<Comment> ChildComments { get; set; } = new List<Comment>();



        private Comment() { }

        public static Comment Create(Guid userId, Guid postId, string content)
        {
            // Create the instance
            var comment = new Comment
            {
                PostId = postId,
                CommentedUserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            return comment;
        }

        public void Update(string content) => Content = content;
    }
}
