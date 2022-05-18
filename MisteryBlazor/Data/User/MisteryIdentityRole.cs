using Microsoft.AspNetCore.Identity;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Data.User
{
    /// <summary>
    /// IdentityRole 的重载，修改了 Id 的生成方式
    /// </summary>
    public class MisteryIdentityRole : IdentityRole
    {
        public MisteryIdentityRole()
        {
            Id = str.GetIntId().ToString();
        }

        public MisteryIdentityRole(string roleName) : this()
        {
            Name = roleName;
        }
    }
}
