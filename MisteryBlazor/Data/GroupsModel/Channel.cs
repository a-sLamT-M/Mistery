using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    /// <summary>
    /// 频道模型，CategoryId 为 ChannelCategory 内的 Id，标记该频道属于哪个类别
    /// </summary>
    public class Channel
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string ChannelName { get; set; }
        [Required, StringLength(200)]
        public int CategoryId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
