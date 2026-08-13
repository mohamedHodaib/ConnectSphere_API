using ConnectSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConnectSphere.Infrastructure.Persistence.Configurations
{
    public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
    {
        public void Configure(EntityTypeBuilder<Bookmark> builder)
        {
            builder.HasKey(b => new { b.PostId, b.BookmarkedUserId });

            builder.HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.BookmarkedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(b => b.Post)
                .WithMany()
                .HasForeignKey(b => b.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(b => new { b.BookmarkedUserId, b.CreatedAt });
        }
    }
}