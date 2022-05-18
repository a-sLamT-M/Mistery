using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    /// <summary>
    /// 群头像模型，标记群（GroupId）的头像是 AvatarStamp
    /// </summary>
    public class GroupAvatar
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string AvatarStamp  { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
