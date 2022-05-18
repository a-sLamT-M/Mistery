using Microsoft.AspNetCore.Identity;
using MisteryBlazor.StringUtils;
using System.ComponentModel.DataAnnotations;
namespace MisteryBlazor.Data.User
{
    /// <summary>
    /// IdentityUser 的重载，修改了 Id 的生成方式
    /// </summary>
    public class MisteryIdentityUser : IdentityUser
    {
        public MisteryIdentityUser() : base()
        {
            Id = str.GetIntId().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }
        public MisteryIdentityUser(string userName) : this()
        {
            UserName = userName;
        }
        [Timestamp]
        public byte[] CreationTime { get; set; }
    }
}
