using System;
using Microsoft.Data.Entity;

namespace MicroBlogging.Models
{
    public interface IEntity
    {
    }

    public class MicroBloggerContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Follow> Follows { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var computer = Environment.GetEnvironmentVariable("COMPUTERNAME");
            var sqlServer = "SQLEXPRESS";
            var database = "MicroBloggingDb";
            options.UseSqlServer($@"Server={computer}\{sqlServer};Database={database};Trusted_Connection = True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();
            user.HasIndex(u => new {u.Id});
            user.Property(u => u.FullName).IsRequired();
            user.Property(u => u.Username).IsRequired();
            user.Property(u => u.Gender).IsRequired();
            user.Property(u => u.Password).IsRequired();
            user.Ignore(u => u.ConfirmPassword);
            user.ToTable("users");

            var post = modelBuilder.Entity<Post>();
            post.HasIndex(p => new {p.Id});
            post.HasOne(t => t.User);

            post.ToTable("posts");

            var follow = modelBuilder.Entity<Follow>();
            follow.HasIndex(p => new { p.Id });
            follow.ToTable("follows");
        }
    }
}