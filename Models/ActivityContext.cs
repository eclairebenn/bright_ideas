using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace dojo_activities.Models
{
    public class ActivityContext : IdentityDbContext<User>
    {
        public ActivityContext(DbContextOptions<ActivityContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Event> events {get;set;}
        public DbSet<Participant> participants {get;set;}

    }
}