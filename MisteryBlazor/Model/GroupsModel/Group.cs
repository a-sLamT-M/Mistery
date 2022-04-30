using System.ComponentModel.DataAnnotations;

namespace Mistery.Model.GroupsModel
{
    public class Group
    {
        public int GroupId { get; set; }
        [Required, StringLength(100)] 
        public string GroupName { get; set; }
        public List<Channel> Channels { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
