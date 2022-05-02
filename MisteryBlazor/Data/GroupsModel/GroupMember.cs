using MisteryBlazor.Data.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.GroupsModel
{
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomPermissionRoleId { get; set; }
        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [Required]
        [ForeignKey("Member")]
        public string GroupMemberId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public MisteryIdentityUser Member { get; set; }
        public Group Group { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
