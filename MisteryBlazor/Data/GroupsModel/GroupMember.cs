using System.ComponentModel.DataAnnotations;
using MisteryBlazor.Enums;

namespace MisteryBlazor.Data.GroupsModel
{
    /// <summary>
    /// 群成员模型
    /// </summary>
    public class GroupMember
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string GroupMemberId { get; set; }
        [Required]
        public GroupMemberStatus Status { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
