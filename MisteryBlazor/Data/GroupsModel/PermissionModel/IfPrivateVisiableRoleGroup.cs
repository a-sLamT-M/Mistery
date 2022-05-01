using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.GroupsModel.PermissionModel
{
    public class IfPrivateVisiableRoleGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        public List<CustomPermissionRole> CustomPermissionRole { get; set; } = new List<CustomPermissionRole>();
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
