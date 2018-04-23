using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dojo_activities.Models
{
    public class BeltContext : IdentityDbContext<User>
    {
        public BeltContext(DbContextOptions<BeltContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Meet> Meets {get;set;}
        public DbSet<Participant> participants {get;set;}

    }
}