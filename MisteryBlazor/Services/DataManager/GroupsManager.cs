using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Services.DAL;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Services.DataManager
{
    public class GroupsManager
    {
        private ILogger _Logger;
        private GroupDataService _Gps;
        private AuthorizationManager _Am;
        private int selectGroupId = 0;
        public int SelectedGroupId
        {
            set
            {
                selectGroupId = value;
                selectedGroup = _Gps.GetGroupById("RoomMain: Getting Group", value);
            }
        }
        private Group selectedGroup;
        private Dictionary<Group, bool> groupsDictionary;
        private IList<Group> groups;

        public Group SelectedGroup => selectedGroup;
        public IList<Group> Groups => groups;
        public Dictionary<Group, bool> GroupsDictionary => groupsDictionary;
        public GroupsManager(ILogger<GroupsManager> logger, GroupDataService gps, AuthorizationManager am)
        {
            _Logger=logger;
            _Gps = gps;
            _Am = am;
            _ = InitAsync();
        }
        public async Task InitAsync()
        {
            groups = await _Gps.GetGroupsFromUserAsync("RoomUserBar: Getting Groups.", _Am.UserId)!;
            groupsDictionary = await _Gps.CompareIfGroupsIsOnwedByUserAsync("RoomUserBar: Comparing groups with uid.", _Am.UserId, Groups);
        }

        public async Task DeleteGroup(string uid, int gid)
        {

        }
    }
}
