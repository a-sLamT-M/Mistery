using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.User
{
    /// <summary>
    /// 同 GroupAvatar 模型
    /// </summary>
    public class UserAvatar
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
