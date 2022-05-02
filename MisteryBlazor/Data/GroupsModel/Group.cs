using MisteryBlazor.Data.GroupsModel.PermissionModel;
using MisteryBlazor.Data.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.GroupsModel
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string GroupName { get; set; }
        [Required]
        [ForeignKey("Owner")]
        public string GroupOwnerId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        public MisteryIdentityUser Owner { get; set; }
        public List<Channel> Channels { get; set; }
        public List<CustomPermissionRole> Roles { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
