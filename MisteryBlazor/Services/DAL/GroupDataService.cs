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
    /// <summary>
    /// 关于群组的数据访问层，
    /// </summary>
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

        /// <summary>
        /// 获取整个 UserInRoles 表
        /// </summary>
        /// <param name="log"></param>
        /// <returns>UserInRoles List</returns>
        public List<UserInRole> GetAllUserInRoles(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return UserInRoles.ToList();
        }
        /// <summary>
        /// 获取整个 Groups 表
        /// </summary>
        /// <param name="log"></param>
        /// <returns>Groups List</returns>
        public List<Group> GetAllGroups(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return groups;
        }
        /// <summary>
        /// 获取整个 GroupAvatars 表
        /// </summary>
        /// <param name="log"></param>
        /// <returns>GroupAvatars List</returns>
        public List<GroupAvatar> GetAllGroupAvatar(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return GroupsAvatars;
        }

        public async Task<IEnumerable<GroupAvatar>>? GetAllGroupAvatarAsync(string log, string uid)
        {
            var groups = GetGroupsFromUser(log, uid);
            var groupsAvatar = _context.GroupAvatars.ToList();
            return (from a in groupsAvatar
                    where (from g in groups select g.Id).Equals(a.GroupId)
                    select a);
        }

        /// <summary>
        /// 获取整个 GroupMember 表
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <returns>GroupMembers List</returns>
        public List<GroupMember> GetAllGroupsMember(string log, int gid)
        {
            var groupsMember =
                from gp in GroupMembers where gp.GroupId == gid select gp;
            _logger.LogInformation(string.Empty, log);
            return (List<GroupMember>)groupsMember;
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
        /// <summary>
        /// 比较给定的群组是否为给定的 uid 用户所有
        /// </summary>
        /// <param name="log"></param>
        /// <param name="uid"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public KeyValuePair<Group,bool> CompareIfGroupIsOnwedByUser(string log, string uid, Group group)
        {
            return new KeyValuePair<Group, bool>(@group, @group.GroupOwnerId == uid);
        }
        /// <summary>
        /// 比较给定群组列表内每个群组是否为给定的 uid 用户所有
        /// </summary>
        /// <param name="log"></param>
        /// <param name="uid"></param>
        /// <param name="groups"></param>
        /// <returns>Group and Bool Dictionary</returns>
        public Dictionary<Group, bool> CompareIfGroupsIsOnwedByUser(string log, string uid,
            IList<Group> groups)
        {
            return groups.ToDictionary(group => group, group => group.GroupOwnerId == uid);
        }
        /// <summary>
        /// 根据用户 uid 获取所有用户所在的群
        /// </summary>
        /// <param name="log"></param>
        /// <param name="uid"></param>
        /// <returns>Groups List</returns>
        public List<Group>? GetGroupsFromUser(string log, string uid)
        {
            HashSet<int> groupId = new HashSet<int>(GroupMembers
                .Where(gm=>gm.GroupMemberId == uid)
                .Select(g =>g.GroupId));
            var groupsSelected = groups.Where(m => groupId.Contains(m.Id));
            _logger.LogInformation(string.Empty, log);
            return groupsSelected.ToList();
        }
        /// <summary>
        /// 根据群的 gid 获取整个群模型
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <returns>Group</returns>
        public Group? GetGroupById(string log, int gid)
        {
            _logger.LogInformation(string.Empty, log);
            return groups.Find(g => g.Id == gid);
        }
        public async Task<Group> GetGroupByIdAsync(string log, int gid)
        {
            return groups.Find(g => g.Id == gid);
        }
        /// <summary>
        /// 获取整个 GroupsMember 表内数据
        /// </summary>
        /// <param name="log"></param>
        /// <returns>GroupsMember List</returns>
        public List<GroupMember> GetAllGroupsMemberList(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return GroupMembers;
        }
        /// <summary>
        /// 创建一个群，并返回新创建的群实体
        /// </summary>
        /// <param name="log"></param>
        /// <param name="groupName"></param>
        /// <param name="createrId"></param>
        /// <returns>Group EntityEntry</returns>
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

        /// <summary>
        /// 异步地伪删除一个群组
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// 更新群组名字
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <param name="newName"></param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateGroupName(string log, int gid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
                if (!GroupMemberVerification("UpdatingName: Verification working.", gid, uid)) return;
            var needUpdate = _context.Groups.Single(g => g.Id == gid);
            needUpdate.GroupName = newName.ToASCIIByte();
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// 判断用户是否为某群群主
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public bool IsUserOwnedGroup(string log, int gid, string uid)
        {
            return groups.Single(g => g.Id == gid).GroupOwnerId == uid;
        }
        /// <summary>
        /// 获取整个 ChannelMessages 表
        /// </summary>
        /// <param name="log"></param>
        /// <returns>ChannelMessages List</returns>
        public List<ChannelMessage> GetAllMessages(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return ChannelMessages;
        }
        /// <summary>
        /// 获取整个 Channels 表
        /// </summary>
        /// <param name="log"></param>
        /// <returns>Channel List</returns>
        public List<Channel> GetAllChannels(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return Channels;
        }
        /// <summary>
        /// 获取整个 ChannelCategories 表
        /// </summary>
        /// <param name="log"></param>
        /// <returns>ChannelCategory List</returns>
        public List<ChannelCategory> GetAllChannelCategories(string log)
        {
            _logger.LogInformation(string.Empty, log);
            return ChannelCategorys.ToList();
        }
        /// <summary>
        /// 根据群 gid 获取该群所有频道
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <returns>Channel List</returns>
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
        /// <summary>
        /// 异步地根据群 gid 获取该群所有频道
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <returns>Channel List</returns>
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
        /// <summary>
        /// 根据 cid 获取对应的频道模型
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cid"></param>
        /// <returns>Channel</returns>
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
        /// <summary>
        /// 异步地根据群 gid 获取所有群类别
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <returns>ChannelCategory List</returns>
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
        /// <summary>
        /// 异步地根据群 gid 获取频道类别以及类别内所有群
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <returns>ChannelCategory, Channel IList</returns>
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
        /// <summary>
        /// 创建类别，并返回创建好的类别实体
        /// </summary>
        /// <param name="log"></param>
        /// <param name="uid"></param>
        /// <param name="gid"></param>
        /// <param name="name"></param>
        /// <returns>ChannelCategory EntityEntry</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// 异步地伪删除类别
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// 创建频道，并返回已创建好的频道实体
        /// </summary>
        /// <param name="log"></param>
        /// <param name="uid"></param>
        /// <param name="gid"></param>
        /// <param name="cid"></param>
        /// <param name="name"></param>
        /// <returns>Channel EntityEntry</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// 异步地伪删除频道
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
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
        /// <summary>
        /// 异步地更新类别名称
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <param name="newName"></param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateCategoryNameAsync(string log, int cid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
            if (!GroupMemberVerification("UpdatingName: Verification working.", cid, uid)) return;
            var needUpdate = _context.ChannelCategories.Single(g => g.Id == cid);
            needUpdate.CategoryName = newName.ToASCIIByte();
            await _context.SaveChangesAsync();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
        }
        /// <summary>
        /// 异步地更新频道抿成
        /// </summary>
        /// <param name="log"></param>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <param name="newName"></param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateChannelNameAsync(string log, int cid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) throw new Exception("Group name.length >=StringMarcos.MAX_STRING_LENGTH");
            if (!GroupMemberVerification("UpdatingName: Verification working.", cid, uid)) return;
            var needUpdate = _context.Channels.Single(g => g.Id == cid);
            needUpdate.ChannelName = newName.ToASCIIByte();
            await _context.SaveChangesAsync();
            Channels = _context.Channels.ToList();
            ChannelCategorys = _context.ChannelCategories.ToHashSet();
        }
        /// <summary>
        /// 群成员操作权限验证
        /// </summary>
        /// <param name="log"></param>
        /// <param name="gid"></param>
        /// <param name="uid"></param>
        /// <returns>bool</returns>
        public bool GroupMemberVerification(string log, int gid, string uid)
        {
            return IsUserOwnedGroup(log, gid, uid);
        }
    }
}
