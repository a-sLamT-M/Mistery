using System.Data.Common;
using System.Text;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Services.DAL;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Services.DataManager
{
    public class ChannelsManager
    {
        private ILogger _Logger;
        private GroupDataService _Gps;
        private AuthorizationManager _Am;
        private GroupsManager _Gm;
        private KeyValuePair<Group, bool> CurrectGroup;
        private IList<ChannelCategory> _ChannelCategories;
        private IList<Channel> _Channels;
        private Dictionary<ChannelCategory, IList<Channel>> _ChannelsDictionary = new();
        public delegate Task CategoryUpdated();
        public event CategoryUpdated CategoryUpdateEvent;
        public Dictionary<ChannelCategory,IList<Channel>> ChannelsDictionary
        {
            get => _ChannelsDictionary;
        }
        public ChannelsManager(ILogger<ChannelsManager> logger, GroupDataService gps, AuthorizationManager Am, GroupsManager Gm)
        {
            _Logger = logger;
            _Gps= gps;
            _Am= Am;
            _Gm = Gm;
            _ = InitAsync();
        }

        public async Task InitAsync()
        {
            CurrectGroup = _Gm.SelectedGroup;
            if (CurrectGroup.Key is not null) 
            {
                _Channels = await _Gps.GetChannelFromGroupAsync("Loading Channels.", CurrectGroup.Key.Id)!;
                _ChannelCategories = await _Gps.GetChannelCatagoryFromGroupAsync("Loading Channel categories List", CurrectGroup.Key.Id)!;
                _ChannelsDictionary =
                    await _Gps.GetChannelWithCatagoryFromGroupAsync("Loading Channel map", CurrectGroup.Key.Id);
            }
        }

        public async Task<int> CreateCategory(string categoryName, string uid, int gid)
        {
            if (categoryName.ToASCIIByte().Length >= 180)
            {
                throw new Exception("Group name is required in 180 chars");
            }
            StringBuilder log = new StringBuilder();
            log.Append("User：").Append(uid).Append(" ").Append("is creating group ").Append(categoryName);
            var result = _Gps.CreateCategory(log.ToString(), uid, gid, categoryName);
            return result.Entity.Id;
        }
        public async Task DeleteCategory(string categoryName, string uid, int cid)
        {
            var sb = new StringBuilder();
            sb.Append(uid).Append(" trying delete channel category ").Append(cid.ToString());
            try
            {
                await _Gps.SetCategoryDeleted(sb.ToString(), cid, uid);
            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
                throw;
            }
        }

        public async Task UpdateCategoryName(string log, int cid, string uid, string newName)
        {
            if (newName.ToASCIIByte().Length >= 180 || newName.Length == 0) throw new Exception("Group name.length >=180");
            try
            {
                await _Gps.UpdateCategoryName(log, cid, uid, newName);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
