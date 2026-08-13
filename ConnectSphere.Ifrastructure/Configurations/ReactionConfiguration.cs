using ConnectSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Configurations
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.ToTable("Reactions");

            builder.HasKey(r => new { r.ReactedUserId, r.PostId });

            builder.Property(r => r.Type)
                .IsRequired();

            //RelationShips
            //With Posts 
            
            builder.HasOne(r => r.Post)
                .WithMany()
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.Cascade); // Deleting the Post removes the Reactions

            //With Reacted User
            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.ReactedUserId)
                .OnDelete(DeleteBehavior.NoAction); // <--- THIS BREAKS THE CYCLE 

            builder.HasIndex(r => new { r.ReactedUserId, r.PostId })
            .IsUnique();
        }
    }
}
