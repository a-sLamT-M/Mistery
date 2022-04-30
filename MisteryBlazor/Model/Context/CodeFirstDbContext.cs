using Microsoft.EntityFrameworkCore;
using Mistery.Model.GroupsModel;
using Mistery.Model.MessagesModel;
using Mistery.Model.Users;

namespace Mistery.Model.DB
{
    public class CodeFirstDbContext : DbContext
    {
        public CodeFirstDbContext(DbContextOptions<CodeFirstDbContext> options) : base(options){}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(new Group { GroupId = 1, GroupName = "MISTERY" });
            modelBuilder.Entity<Channel>().HasData(
                new Channel { ChannelId = 1, ChannelName = "general", GroupId = 1 });
        }

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GroupMember> ServerMembers { get; set; }
    }
}
