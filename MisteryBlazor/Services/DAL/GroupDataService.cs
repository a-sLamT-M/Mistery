using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MisteryBlazor.Data.Context;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.GroupsModel.PermissionModel;
using MisteryBlazor.Data.MessagesModel;
using MisteryBlazor.Data.User;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Services.DAL
{
    public class GroupDataService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        private List<Group> groups;
        private List<GroupMember> GroupMembers;
        private List<ChannelMessage> ChannelMessages;
        private List<Channel> Channels;
        private List<GroupAvatar> GroupsAvatars;
        public GroupDataService(AppDbContext context, ILogger<GroupDataService> logger)
        {
            _context = context;
            _logger = logger;

            ChannelMessages = _context.ChannelMessages.ToList();
            groups = _context.Groups.ToList();
            GroupMembers = _context.GroupMembers.ToList();
            Channels = _context.Channels.ToList();
            GroupsAvatars = _context.GroupAvatars.ToList();
        }

        public List<Group> GetAllGroups(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return groups;
        }
        public List<GroupAvatar> GetAllGroupAvatar(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return GroupsAvatars;
        }
        public async Task<IEnumerable<GroupAvatar>>? GetAllGroupAvatarAsync(string log, string uid)
        {
            var groups = await GetGroupsFromUserAsync(log, uid);
            var groupsAvatar = _context.GroupAvatars.ToList();
            return (from a in groupsAvatar
                    where (from g in groups select g.Id).Equals(a.groupId)
                    select a);
        }
        public List<GroupMember> GetAllGroupsMember(string log, int gid)
        {

            var groupsMember =
                from gp in GroupMembers where gp.GroupId == gid select gp;
            _logger.LogInformation(string.Empty, log);
            return (List<GroupMember>)groupsMember;
        }
        public async Task<List<Group>>? GetGroupsFromUserAsync(string log, string uid)
        {
            HashSet<int> groupId = new HashSet<int>(GroupMembers
                .Where(gm => gm.GroupMemberId == uid)
                .Select(g => g.GroupId));
            var groupsSelected = groups.Where(m => groupId.Contains(m.Id));
            _logger.LogInformation(string.Empty, log);

            return groupsSelected.ToList();
        }

        public async Task<Dictionary<Group, bool>> CompareIfGroupsIsOnwedByUserAsync(string log, string uid,
            IList<Group> groups)
        {
            Dictionary<Group, bool> groupsWithBools = new Dictionary<Group, bool>();
            foreach (var group in groups)
            {
                if (group.GroupOwnerId == uid)
                {
                    groupsWithBools.Add(group, true);
                }
                else
                {
                    groupsWithBools.Add(group, false);
                }
            }
            return groupsWithBools;
        }
        public Dictionary<Group, bool> CompareIfGroupsIsOnwedByUser(string log, string uid,
            IList<Group> groups)
        {
            Dictionary<Group, bool> groupsWithBools = new Dictionary<Group, bool>();
            foreach (var group in groups)
            {
                if (group.GroupOwnerId == uid)
                {
                    groupsWithBools.Add(group, true);
                }
                else
                {
                    groupsWithBools.Add(group, false);
                }
            }
            return groupsWithBools;
        }
        public List<Group>? GetGroupsFromUser(string log, string uid)
        {
            HashSet<int> groupId = new HashSet<int>(GroupMembers
                .Where(gm=>gm.GroupMemberId == uid)
                .Select(g =>g.GroupId));
            var groupsSelected = groups.Where(m => groupId.Contains(m.Id));
            _logger.LogInformation(string.Empty, log);
            return groupsSelected.ToList();
        }
        public List<Channel>? GetChannelFromGroup(string log, int gid)
        {
            try
            {
                var channelSelected = Channels.Where(m => m.GroupId == gid);
                _logger.LogInformation(string.Empty, log);
                return channelSelected.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Channel>();
            }
        }
        public async Task<List<Channel>>? GetChannelFromGroupAsync(string log, int gid)
        {
            try
            {
                var channelSelected = Channels.Where(m => m.GroupId == gid);
                _logger.LogInformation(string.Empty, log);
                return channelSelected.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<Channel>();
            }
        }
        public Group GetGroupById(string log, int gid)
        {
            return groups.Find(g => g.Id == gid);
        }
        public async Task<Group> GetGroupByIdAsync(string log, int gid)
        {
            return groups.Find(g => g.Id == gid);
        }
        public List<GroupMember> GetAllGroupsMemberList(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return GroupMembers;
        }
        public List<ChannelMessage> GetAllMessages(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return ChannelMessages;
        }
        public List<Channel> GetAllChannels(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return Channels;
        }
        public EntityEntry<Group> CreateNewGroup(string log, string groupName, string createrId)
        {
            _logger.LogInformation(string.Empty, log);

            // add a mew group
            var newGroup = _context.Groups.Add(new Group()
            {
                GroupName = groupName.ToASCIIByte(),
                GroupOwnerId = createrId,
                IsDeleted = false
            });
            _context.SaveChanges();
            // for this new group, add a new role named Everyone as a default role
            var newGroupDefaultRole = _context.CustomPermissionRoles.Add(new CustomPermissionRole()
            {
                GroupId = newGroup.Entity.Id,
                CustomPermissionRoleName = "Everyone"
            });
            _context.SaveChanges();
            // for this new group, add a new role named Administrator as a op role
            var newGroupAdministratorRole = _context.CustomPermissionRoles.Add(new CustomPermissionRole()
            {
                GroupId = newGroup.Entity.Id,
                CustomPermissionRoleName = "Administrator",
                CanSendMessage = true,
                HaveOP = true,
                CanViewChannel = true,
                CanManageChannel = true
            });
            _context.SaveChanges();
            // add creator to group member table
            var creator = _context.GroupMembers.Add(new GroupMember()
            {
                GroupId = newGroup.Entity.Id,
                GroupMemberId = createrId,
                IsDeleted = false
            });
            _context.SaveChanges();
            // add creator to administrator role
            var UserInRole = _context.UserInRoles.Add(new UserInRole()
            {
                Uid = createrId,
                RoleId = newGroupAdministratorRole.Entity.Id,
                GroupId = newGroup.Entity.Id
            }
            );
            _context.SaveChanges();

            //refresh this->groups (GroupDataService::Groups)
            //refresh this->GroupMembers (GroupDataService::GroupMembers)
            groups = _context.Groups.ToList();
            GroupMembers = _context.GroupMembers.ToList();
            return newGroup;
        }
    }
}
