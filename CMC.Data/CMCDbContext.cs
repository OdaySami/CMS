using CMC.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMC.Data
{
    public class CMCDbContext : IdentityDbContext<User>
    {
        public CMCDbContext(DbContextOptions<CMCDbContext> options)
            : base(options)
        { 
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostAttachment> PostAttachments { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }
        public DbSet<ContentChangeLog> ContentChangeLogs { get; set; }

        public object Include(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
