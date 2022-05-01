using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MisteryBlazor.Data.User
{
    public class Relation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Requestor")]
        public int RequestorId { get; set; }
        [Required]
        [ForeignKey("Receiver")]
        public int ReceiverId { get; set; }
        [Required]
        public string Status { get; set; }
        public MisteryIdentityUser Requestor { get; set; }
        public MisteryIdentityUser Receiver { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
