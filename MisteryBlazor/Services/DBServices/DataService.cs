using Microsoft.EntityFrameworkCore;
using MisteryBlazor.Data.Context;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Data.User;

namespace MisteryBlazor.Services.MessageServices
{
    public class DataService : IDbService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public DataService(AppDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<MisteryIdentityUser>> GetAllUsers(string log)
        {
            var users = await _context.MisteryUsers.ToListAsync();
            _logger.LogInformation(string.Empty, log);
            return users;
        }
        public async Task<List<UserAvatars>> GetAllUsersAvatars(string log)
        {
            var usersAvatars = await _context.UserAvatars.ToListAsync();
            _logger.LogInformation(string.Empty, log);
            return usersAvatars;
        }
        public async Task<List<Group>> GetAllGroups(string log)
        {
            var groups = await _context.Groups.ToListAsync();
            _logger.LogInformation(string.Empty, log);
            return groups;
        }
        public async Task<List<GroupMember>> GetAllGroupsMember(string log, int gid)
        {
            var groupsMemberList = await _context.GroupMembers.ToListAsync();
            var groupsMember =
                from gp in groupsMemberList where gp.GroupId == gid select gp;
            _logger.LogInformation(string.Empty, log);
            return (List<GroupMember>)groupsMember;
        }
        public async Task<List<GroupMember>> GetAllGroupsMemberList(string log)
        {
            var groupsMemberList = await _context.GroupMembers.ToListAsync();
            _logger.LogInformation(string.Empty, log);
            return groupsMemberList;
        }
        public async Task<List<Channel>> GetAllChannels(string log)
        {
            var channel = await _context.Channels.ToListAsync();
            _logger.LogInformation(string.Empty, log);
            return channel;
        }
    }
}
