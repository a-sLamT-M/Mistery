using System.ComponentModel.DataAnnotations;
using Mistery.Model.GroupsModel;
using Mistery.Model.Users;

namespace Mistery.Model.MessagesModel
{
    public class Message
    {
        public int MessageId { get; set; }
        [Required]
        public int ChannelId { get; set; }
        public Channel Channel { get; set; }
        [Required]
        public int UserId { get; set; }
        public User User { get; set; }
        [StringLength(10000)]
        public string MessageContent { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
