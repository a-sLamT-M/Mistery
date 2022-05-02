using Microsoft.AspNetCore.Identity;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Data.User
{
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
