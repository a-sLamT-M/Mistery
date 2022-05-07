using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel.PermissionModel
{
    public class CustomPermissionRole
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string CustomPermissionRoleName { get; set; } = "Everyone";

        // ------------------------------------------------------------------------
        public bool CanSendMessage { get; set; } = true;
        public bool HaveOP { get; set; } = false;
        public bool CanViewChannel { get; set; } = true;
        public bool CanManageChannel { get; set; } = false;
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
