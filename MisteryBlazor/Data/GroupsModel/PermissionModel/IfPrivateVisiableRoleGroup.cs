using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel.PermissionModel
{
    public class IfPrivateVisiableRoleGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ChannelId { get; set; }
        public int RoleId { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
