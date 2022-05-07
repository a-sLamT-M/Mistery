using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.User
{
    public class Relation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RequestorId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public string Status { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
