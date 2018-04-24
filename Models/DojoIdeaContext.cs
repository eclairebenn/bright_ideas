using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bright_ideas.Models
{
    public class BIdeaContext : IdentityDbContext<User>
    {
        public BIdeaContext(DbContextOptions<BIdeaContext> options) : base(options) { }

        public DbSet<User> users { get; set; }

        public DbSet<Idea> ideas {get;set;}
        public DbSet<Like> likes {get;set;}

    }
}