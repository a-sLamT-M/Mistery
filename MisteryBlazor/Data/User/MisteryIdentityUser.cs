using Microsoft.AspNetCore.Identity;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.MessagesModel;
using MisteryBlazor.StringUtils;
using System.ComponentModel.DataAnnotations;
namespace MisteryBlazor.Data.User
{
    //Cannot insert explicit value for identity column in table when IDENTITY_INSERT is set to OFF.
    public class MisteryIdentityUser : IdentityUser
    {
        public IList<Group> Groups { get; set; } = new List<Group>();
        public IList<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();
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
