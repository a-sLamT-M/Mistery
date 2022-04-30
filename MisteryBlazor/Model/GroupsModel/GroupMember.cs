using System.ComponentModel.DataAnnotations;
using Mistery.Model.Users;

namespace Mistery.Model.GroupsModel
{
    public class GroupMember
    {
        public int GroupMemberId { get; set; }
        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
