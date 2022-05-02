using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.User
{
    public class UserAvatars
    {
        [Key]
        public string Id { get; set; }
        [ForeignKey("Sender")]
        public string Uid { get; set; }
        public MisteryIdentityUser Sender { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
