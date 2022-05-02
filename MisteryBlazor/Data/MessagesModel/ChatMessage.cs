using MisteryBlazor.Data.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.MessagesModel
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [ForeignKey("Target")]
        public string TargetUserId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        public MisteryIdentityUser Sender { get; set; }
        public MisteryIdentityUser Target { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
