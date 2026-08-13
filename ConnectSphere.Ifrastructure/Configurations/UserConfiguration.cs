using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectSphere.Domain.Entities;

namespace ConnectSphere.Infrastructure.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.Biography)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(u => u.ProfilePictureUrl)
                .HasMaxLength(2048)
                .IsRequired(false); // Standard URL max length

            builder.Ignore(u => u.DisplayName);

            builder.Property(u => u.Email)
                .IsRequired();

            //username
            builder.Property(u => u.UserName)
                .IsRequired();

            builder.HasIndex(u => u.UserName)
                .IsUnique();


            //RelationShips

            //with posts
            builder.HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
