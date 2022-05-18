using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.MessagesModel
{
    /// <summary>
    /// 频道消息模型
    /// </summary>
    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public int ChannelId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
