using Microsoft.EntityFrameworkCore;
using MisteryBlazor.Data.Context;
using MisteryBlazor.Data.MessagesModel;
using MisteryBlazor.Data.User;

namespace MisteryBlazor.Services.DAL
{
    public class UserDataService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public UserDataService(AppDbContext context, ILogger<UserDataService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<MisteryIdentityUser> GetAllUsers(string log)
        {
            var users = _context.MisteryUsers.ToList();
            _logger.LogInformation(string.Empty, log);
            return users;
        }
        public List<UserAvatars> GetAllUsersAvatars(string log)
        {
            var usersAvatars = _context.UserAvatars.ToList();
            _logger.LogInformation(string.Empty, log);
            return usersAvatars;
        }
        public List<ChatMessage> GetAllMessagesList(string log)
        {
            var cm = _context.ChatMessages.ToList();
            _logger.LogInformation(string.Empty, log);
            return cm;
        }
        public List<ChatMessage> GetAllMessagesFromSender(string log, string senderId)
        {
            var cm = _context.ChatMessages.ToList();
            var m =
                from c in cm
                where c.SenderId == senderId
                select c;
            _logger.LogInformation(string.Empty, log);
            return (List<ChatMessage>)m;
        }
        public List<ChatMessage> GetAllMessagesFromUsers(string log, string senderId, string targetId)
        {
            var cm = _context.ChatMessages.ToList();
            var m =
                from c in cm
                where c.SenderId == senderId && c.TargetUserId == targetId
                select c;
            _logger.LogInformation(string.Empty, log);
            return (List<ChatMessage>)m;
        }
        public List<Relation> GetAllRelations(string log, string uid)
        {
            var cm = _context.Relations.ToList();
            var m =
                from c in cm
                where c.ReceiverId == uid
                select c;
            _logger.LogInformation(string.Empty, log);
            return (List<Relation>)m;
        }
        // unfinished
        public void SetAvatars(string uid, string avatars)
        {
            DbSet<UserAvatars> avatarsList = _context.UserAvatars;
        }
    }
}
