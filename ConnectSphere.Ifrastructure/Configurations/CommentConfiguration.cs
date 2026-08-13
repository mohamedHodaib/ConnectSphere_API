using ConnectSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comments");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Content)
                .IsRequired();

            builder.Property(c => c.CommentedUserId)
                .IsRequired();

            builder.Property(c => c.PostId)
                .IsRequired();

            //RelationShips
            //With Posts 

            builder.HasOne(r => r.Post)
                .WithMany()
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting the Post removes the Comments

            //With Commented User
            builder.HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.CommentedUserId)
                .OnDelete(DeleteBehavior.NoAction); 

            //With Parent Comment if exist
            builder.HasOne(cc => cc.ParentComment)
                .WithMany(pc => pc.ChildComments)
                .HasForeignKey(cc => cc.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
