using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.User
{
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