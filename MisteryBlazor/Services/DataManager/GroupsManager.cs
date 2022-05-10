using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components;
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
                var g = _Gps.GetGroupById("RoomMain: Getting Group", value);
                if (g is not null)
                {
                    selectedGroup=_Gps.CompareIfGroupIsOnwedByUser("RoomUserBar: Comparing group with uid.", _Am.UserId, g);
                }
            }
        }
        private KeyValuePair<Group,bool> selectedGroup;
        private Dictionary<Group, bool> groupsDictionary;
        private IList<Group> groups;
        public delegate Task GroupNameUpdated(int id);
        public event GroupNameUpdated GroupNameUpdatedEvent;

        public KeyValuePair<Group, bool> SelectedGroup => selectedGroup;
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

        public int CreateGroup(string uid, string groupName)
        {
            if (groupName.ToASCIIByte().Length >= 180)
            {
                throw new Exception("Group name is required in 180 chars");
            }
            StringBuilder log = new StringBuilder();
            log.Append("User：").Append(uid).Append(" ").Append("is creating group ").Append(groupName);
            var result = _Gps.CreateNewGroup(log.ToString(), groupName, uid);
            return result.Entity.Id;
        }

        public async Task DeleteGroup(string uid, int gid)
        {
            var sb = new StringBuilder();
            sb.Append(uid).Append(" trying delete group ").Append(gid.ToString());
            try
            {
                await _Gps.SetGroupDeleted(sb.ToString(), gid, uid);
            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
                throw;
            }
        }

        public async Task UpdateGroupName(int gid, string uid, string newName)
        {
            var sb = new StringBuilder();
            sb.Append("Group ").Append(gid).Append("updating name: ").Append(newName);
            try
            {
                SelectedGroupId = gid;
                GroupNameUpdatedEvent(gid);
                await _Gps.UpdateGroupName(sb.ToString(), gid, uid, newName);
            }
            catch(Exception e)
            {
                _Logger.LogError(e.Message);
                throw;
            }
        }
    }
}
