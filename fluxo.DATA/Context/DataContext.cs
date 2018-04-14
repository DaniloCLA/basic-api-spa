

using fluxo.DATA.Models;
using Microsoft.EntityFrameworkCore;

namespace fluxo.DATA.Context
{
    public class DataContext : DbContext
    {        
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<TeamAssignment> TeamAssignments { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TeamAssignment>()
                .HasKey(k => new {k.UserId, k.TeamId});

            builder.Entity<User>()
                .HasOne(u => u.OrganizationOwned)
                .WithOne(o => o.Owner)
                .HasForeignKey<Organization>(o => o.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(u => u.TeamsAssigned)
                .WithOne(ta => ta.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Team>()
                .HasOne(o => o.Organization)
                .WithMany(t => t.Teams)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Team>()
                .HasMany(t => t.UsersAssigned)
                .WithOne(ta => ta.Team)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}