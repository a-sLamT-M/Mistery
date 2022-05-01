using Microsoft.AspNetCore.Identity;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Data.User
{
    public class MisteryIdentityRole : IdentityRole<int>
    {

        public MisteryIdentityRole()
        {
            Id = str.GetIntId();
        }

        public MisteryIdentityRole(string roleName) : this()
        {
            Name = roleName;
        }
    }
}
