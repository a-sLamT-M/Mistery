using System.ComponentModel.DataAnnotations;

namespace Mistery.Model.MessagesModel
{
    public class ChatMessage
    {
        public string UserName { get; set; }
        public string MessageContent { get; set; }
        public string ChannelId { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
