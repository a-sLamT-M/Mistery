using System.ComponentModel.DataAnnotations;
using Mistery.Model.MessagesModel;

namespace Mistery.Model.GroupsModel
{
    public class Channel
    {
        public int ChannelId { get; set; }
        [Required, StringLength(100)]
        public string ChannelName { get; set; }
        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public List<Message> Messages { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
