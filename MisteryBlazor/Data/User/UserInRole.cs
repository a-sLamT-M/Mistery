using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.User
{
    /// <summary>
    /// 用户所在组模型，RoleId 为群 GroupId 的全局组
    /// </summary>
    public class UserInRole
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Required]
        public int RoleId { get; set; }
        public int GroupId { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}