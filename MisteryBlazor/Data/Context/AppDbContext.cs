using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.GroupsModel.PermissionModel;
using MisteryBlazor.Data.MessagesModel;
using MisteryBlazor.Data.User;

namespace MisteryBlazor.Data.Context
{
    /// <summary>
    /// 数据库上下文，操作数据库的接口
    /// </summary>
    public class AppDbContext : IdentityDbContext<MisteryIdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<MisteryIdentityUser> MisteryUsers { get; set; }
        public DbSet<UserAvatar> UserAvatars { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupAvatar> GroupAvatars { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChannelMessage> ChannelMessages { get; set; }
        public DbSet<CustomPermissionRole> CustomPermissionRoles { get; set; }
        public DbSet<IfPrivateVisiableRoleGroup> IfPrivateVisiableGroups { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<UserInRole> UserInRoles { get; set; }
        public DbSet<ChannelCategory> ChannelCategories { get; set; }
    }
}
