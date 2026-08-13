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
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");

            builder.HasKey(p => p.Id);

            //RelationShips
            
            //With tags
            builder.HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity(eb => eb.ToTable("PostTag"));

            //With post image
            builder.HasMany(p => p.Images)
                .WithOne(pi => pi.Post)
                .HasForeignKey(pi => pi.PostId);

        }
    }
}
