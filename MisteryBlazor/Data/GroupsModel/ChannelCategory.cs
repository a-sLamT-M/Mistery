using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    public class ChannelCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required] 
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Timestamp]
        public byte[] Created { get; set; }
    }
}
