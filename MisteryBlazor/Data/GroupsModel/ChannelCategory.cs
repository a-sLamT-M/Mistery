using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel
{
    /// <summary>
    /// 频道类别模型，GroupId 标记该类别属于哪个群
    /// </summary>
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
