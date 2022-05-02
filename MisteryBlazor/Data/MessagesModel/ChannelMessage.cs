using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.User;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.MessagesModel
{
    public class ChannelMessage
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [ForeignKey("Channel")]
        public int ChannelId { get; set; }
        [Required]
        public string MessageContent { get; set; }
        public Channel Channel { get; set; }
        public MisteryIdentityUser Sender { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
