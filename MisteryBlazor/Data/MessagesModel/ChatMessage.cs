using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.MessagesModel
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string TargetUserId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
