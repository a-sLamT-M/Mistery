using Microsoft.AspNetCore.Identity;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.MessagesModel;
using MisteryBlazor.StringUtils;
namespace MisteryBlazor.Data.User
{
    //Cannot insert explicit value for identity column in table when IDENTITY_INSERT is set to OFF.
    public class MisteryIdentityUser : IdentityUser<int>
    {
        public string Avatar { get; set; }
        public IList<Group> Groups { get; set; } = new List<Group>();
        public IList<ChannelMessage> ChannelMessages { get; set; } = new List<ChannelMessage>();
        public MisteryIdentityUser() : base()
        {
            Id = str.GetIntId();
            SecurityStamp = Guid.NewGuid().ToString();
        }
    }
}
