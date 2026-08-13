using ConnectSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectSphere.Infrastructure.Configurations
{
    internal class FollowConfiguration : IEntityTypeConfiguration<Follow>
    {
        public void Configure(EntityTypeBuilder<Follow> builder)
        {
            builder.ToTable("Follows");

            // Composite Primary Key (A user can't follow the same person twice)
            builder.HasKey(f => new { f.FollowerId, f.FollowedUserId });

            builder.HasIndex(f => f.FollowerId);
            builder.HasIndex(f => f.FollowedUserId);

            builder.HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.FollowedUser)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}