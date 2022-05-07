using Microsoft.AspNetCore.Identity;
using MisteryBlazor.StringUtils;
using System.ComponentModel.DataAnnotations;
namespace MisteryBlazor.Data.User
{
    //Cannot insert explicit value for identity column in table when IDENTITY_INSERT is set to OFF.
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
