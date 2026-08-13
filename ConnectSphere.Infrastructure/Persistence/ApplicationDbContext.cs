using ConnectSphere.Application.Common.Interfaces;
using ConnectSphere.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Infrastructure.Persistence
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser>
    {
        //public DbSet<Post> Posts => Set<Post>();

        //public DbSet<Comment> Comments => Set<Comment>();

        //public DbSet<Tag> Tags => Set<Tag>();

        //public DbSet<PostTag> PostTags => Set<PostTag>();

        //public DbSet<Reaction> Reactions => Set<Reaction>();

        //public DbSet<Following> Followings => Set<Following>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
