using MisteryBlazor.Data.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.MessagesModel
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; }
        public MisteryIdentityUser Sender { get; set; }
        public MisteryIdentityUser Target { get; set; }
        [ForeignKey("Sender")]
        public int SenderId { get; set; }
        [ForeignKey("Target")]
        public int TargetUserId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
