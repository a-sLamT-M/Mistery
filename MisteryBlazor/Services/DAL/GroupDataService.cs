using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MisteryBlazor.Data.Context;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.GroupsModel.PermissionModel;
using MisteryBlazor.Data.MessagesModel;
using MisteryBlazor.Data.User;
using MisteryBlazor.Enums;
using MisteryBlazor.Marcos;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Services.DAL
{
    public class GroupDataService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;
        private List<Group> groups;
        private List<GroupMember> GroupMembers;
        private List<GroupAvatar> GroupsAvatars;
        private List<ChannelMessage> ChannelMessages;
        private List<Channel> Channels;
        private HashSet<ChannelCategory> ChannelCategorys;
        private HashSet<UserInRole> UserInRoles;

        public GroupDataService(AppDbContext context, ILogger<GroupDataService> logger)
        {
            _context = context;
            _logger = logger;

            ChannelMessages = _context.ChannelMessages.ToList();
            groups = _context.Groups.ToList();
            GroupMembers = _context.GroupMembers.ToList();
            GroupsAvatars = _context.GroupAvatars.ToList();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
            UserInRoles = _context.UserInRoles.ToHashSet();
        }
        public List<UserInRole> GetAllUserInRoles(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return UserInRoles.ToList();
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
        public KeyValuePair<Group,bool> CompareIfGroupIsOnwedByUser(string log, string uid, Group group)
        {
            return new KeyValuePair<Group, bool>(@group, @group.GroupOwnerId == uid);
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
        public Group GetGroupById(string log, int gid)
        {
            _logger.LogInformation(string.Empty, log);
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
                Status = GroupMemberStatus.Accepted
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

        public async Task SetGroupDeletedAsync(string log, int gid, string uid)
        {
            if (!GroupMemberVerification("UpdatingName: Verification working.", gid, uid))
            {
                throw new Exception("Unable to Set Delete: Currect user is not the group owner");
            }

            var needUpdate = _context.Groups.Single(g => g.Id == gid);
            if (!needUpdate.IsDeleted)
            {
                needUpdate.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation(string.Empty, log);
        }
        public async Task UpdateGroupName(string log, int gid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
                if (!GroupMemberVerification("UpdatingName: Verification working.", gid, uid)) return;
            var needUpdate = _context.Groups.Single(g => g.Id == gid);
            needUpdate.GroupName = newName.ToASCIIByte();
            await _context.SaveChangesAsync();
        }
        public bool IsUserOwnedGroup(string log, int gid, string uid)
        {
            return groups.Single(g => g.Id == gid).GroupOwnerId == uid;
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
        public List<ChannelCategory> GetAllChannelCategories(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return ChannelCategorys.ToList();
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

        public Channel GetChannelById(string log, int cid)
        {
            _logger.LogInformation(string.Empty, log);
            return Channels.Find(c => c.Id == cid);
        }
        public async Task<Channel> GetChannelByIdAsync(string log, int cid)
        {
            _logger.LogInformation(string.Empty, log);
            return Channels.Find(c => c.Id == cid);
        }
        public async Task<List<ChannelCategory>>? GetChannelCatagoryFromGroupAsync(string log, int gid)
        {
            try
            {
                var channelSelected = ChannelCategorys.Where(m => m.GroupId == gid);
                _logger.LogInformation(string.Empty, log);
                return channelSelected.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new List<ChannelCategory>();
            }
        }

        public async Task<Dictionary<ChannelCategory, IList<Channel>>> GetChannelWithCatagoryFromGroupAsync(string log,
            int gid)
        {
            _logger.LogInformation(log);
            Dictionary<ChannelCategory, IList<Channel>> result = new();
            var channels = await GetChannelFromGroupAsync("Loading channels to check category.", gid)!;
            var categories = await GetChannelCatagoryFromGroupAsync("Loading categories to check category.", gid)!;
            foreach (var category in categories)
            {
                if (!result.ContainsKey(category))
                {
                    result.Add(category,new List<Channel>());
                }
            }
            var keyIds = result.Keys.Select(x => x.Id);
            foreach (var channel in channels)
            {
                if (keyIds.Contains(channel.CategoryId))
                {
                    result[result.Keys.Single(x=>x.Id == channel.CategoryId)].Add(channel);
                }
            }

            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                return new();
            }
        }
        public EntityEntry<ChannelCategory> CreateCategory(string log, string uid, int gid, string name)
        {
            _logger.LogInformation(string.Empty, log);
            if (name.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || name.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
            if (!GroupMemberVerification("UpdatingName: Verification working.", gid, uid))
                throw new Exception("User not owned this group.");
            var result =  _context.ChannelCategories.Add(new ChannelCategory()
            {
                GroupId = gid,
                CategoryName = name.ToASCIIByte()
            });
            _context.SaveChanges();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
            return result;
        }
        public async Task SetCategoryDeletedAsync(string log, int cid, string uid)
        {
            if (!GroupMemberVerification("UpdatingName: Verification working.", cid, uid))
            {
                throw new Exception("Unable to Set Delete: Currect user is not the group owner");
            }
            var needUpdate = _context.ChannelCategories.Single(g => g.Id == cid);
            if (!needUpdate.IsDeleted)
            {
                needUpdate.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation(string.Empty, log);
        }
        public EntityEntry<Channel> CreateChannel(string log, string uid, int gid, int cid, string name)
        {
            _logger.LogInformation(string.Empty, log);
            if (name.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || name.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
            if (!GroupMemberVerification("UpdatingName: Verification working.", gid, uid))
                throw new Exception("User not owned this group.");
            var result = _context.Channels.Add(new Channel()
            {
                GroupId = gid,
                CategoryId = cid,
                ChannelName = name.ToASCIIByte()
            });
            _context.SaveChanges();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
            return result;
        }
        public async Task SetChannelDeletedAsync(string log, int cid, string uid)
        {
            if (!GroupMemberVerification("UpdatingName: Verification working.", cid, uid))
            {
                throw new Exception("Unable to Set Delete: Currect user is not the group owner");
            }
            var needUpdate = _context.Channels.Single(g => g.Id == cid);
            if (!needUpdate.IsDeleted)
            {
                needUpdate.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
            _logger.LogInformation(string.Empty, log);
        }
        public async Task UpdateCategoryName(string log, int cid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
            if (!GroupMemberVerification("UpdatingName: Verification working.", cid, uid)) return;
            var needUpdate = _context.ChannelCategories.Single(g => g.Id == cid);
            needUpdate.CategoryName = newName.ToASCIIByte();
            await _context.SaveChangesAsync();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
        }
        public async Task UpdateChannelName(string log, int cid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
            if (!GroupMemberVerification("UpdatingName: Verification working.", cid, uid)) return;
            var needUpdate = _context.Channels.Single(g => g.Id == cid);
            needUpdate.ChannelName = newName.ToASCIIByte();
            await _context.SaveChangesAsync();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
        }
        public bool GroupMemberVerification(string log, int gid, string uid)
        {
            return IsUserOwnedGroup(log, gid, uid);
        }
    }
}
