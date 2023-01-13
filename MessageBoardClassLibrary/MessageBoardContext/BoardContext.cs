using MessageBoardClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.MessageBoardContext
{
    public class BoardContext : DbContext
    {
        public BoardContext(DbContextOptions options) : base(options) { }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<SchoolUser> SchoolUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DynamicPost> DynamicPosts { get; set; }
        public DbSet<Template> DynamicTemplates { get; set; }
        public DbSet<DynamicProperty> DynamicMedia { get; set; }
        public DbSet<DbLog> Logs { get; set; }

        override
        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
