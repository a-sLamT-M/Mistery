using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    public class GroupAvatar
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int groupId { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
