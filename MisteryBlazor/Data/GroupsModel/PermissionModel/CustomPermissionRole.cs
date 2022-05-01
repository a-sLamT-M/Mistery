using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.GroupsModel.PermissionModel
{
    public class CustomPermissionRole
    {
        [Key]
        public int Id { get; set; }
        public int BaseCustomPermissionRoleId { get; set; }
        [ForeignKey("BaseCustomPermissionRoleId")]
        public CustomPermissionRole BaseCustomPermissionRole { get; set; }
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        [ForeignKey("IfPrivateVisiableRoleGroup")]
        public int RoleGroupId { get; set; }
        [Required]
        public string CustomPermissionRoleName { get; set; } = "Everyone";
        public IfPrivateVisiableRoleGroup IfPrivateVisiableRoleGroup { get; set; }

        // ------------------------------------------------------------------------
        public bool CanSendMessage { get; set; } = true;
        public bool HaveOP { get; set; } = false;
        public bool CanViewChannel { get; set; } = true;
        public bool CanManageChannel { get; set; } = false;
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
