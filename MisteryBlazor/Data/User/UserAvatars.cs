using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.User
{
    public class UserAvatars
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Uid { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
