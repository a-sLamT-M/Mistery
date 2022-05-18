using System.ComponentModel.DataAnnotations;

namespace MisteryBlazor.Data.GroupsModel.PermissionModel
{
    /// <summary>
    /// 私有频道权限组白名单模型，标记对于设为私密的频道 ChannelId，什么权限组 RoleId 内的成员可以查看
    /// </summary>
    public class IfPrivateVisiableRoleGroup
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ChannelId { get; set; }
        public int RoleId { get; set; }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
