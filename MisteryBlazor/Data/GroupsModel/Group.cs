using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string GroupName { get; set; }
        [Required]
        public string GroupOwnerId { get; set; }
        [Required]
        public bool IsDeleted { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
