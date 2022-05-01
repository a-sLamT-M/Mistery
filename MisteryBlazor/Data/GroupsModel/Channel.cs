using MisteryBlazor.Data.GroupsModel.PermissionModel;
using MisteryBlazor.Data.MessagesModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.GroupsModel
{
    public class Channel
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string ChannelName { get; set; }
        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        [Required]
        public bool IsDeleted { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        public IfPrivateVisiableRoleGroup IfPrivateVisiableRoleGroup { get; set; }
        public Group Group { get; set; }
        public IList<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
