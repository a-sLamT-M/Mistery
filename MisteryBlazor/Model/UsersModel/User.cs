using System.ComponentModel.DataAnnotations;

namespace Mistery.Model.Users
{
    public abstract class User
    {
        public int UserId { get; set; }
        [Required, StringLength(40)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string UserPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public DateTime CreationTime { get; set; }
    }
}
