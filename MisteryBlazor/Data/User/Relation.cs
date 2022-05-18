using System.ComponentModel.DataAnnotations;
using MisteryBlazor.Enums;

namespace MisteryBlazor.Data.User
{
    /// <summary>
    /// 用户关系模型，定义是谁发起的加好友申请、谁接收的加好友申请 <br/>
    /// Status 为该关系状态
    /// </summary>
    public class Relation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string RequestorId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        [Required]
        public RelationStatus Status { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
