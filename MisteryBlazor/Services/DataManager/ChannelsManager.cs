﻿using System.Data.Common;
using System.Text;
using AntDesign.Core.Helpers.MemberPath;
using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Marcos;
using MisteryBlazor.Services.DAL;
using MisteryBlazor.Services.Events;
using MisteryBlazor.StringUtils;

namespace MisteryBlazor.Services.DataManager
{
    public class ChannelsManager
    {
        private ILogger _Logger;
        private GroupDataService _Gps;
        private AuthorizationManager _Am;
        private GroupsManager _Gm;
        private GroupManagerEvents _Gme;
        private ChannelManagerEvents _Cme;
        private KeyValuePair<Group, bool> CurrentGroup;
        private Channel _SelectedChannel;
        private int _SelectedChannelId;
        private IList<ChannelCategory> _ChannelCategories;
        private IList<Channel> _Channels;
        private Dictionary<ChannelCategory, IList<Channel>> _ChannelsDictionary = new();
        public Dictionary<ChannelCategory,IList<Channel>> ChannelsDictionary
        {
            get => _ChannelsDictionary;
        }
        public int SelectedChannelId
        {
            set
            {
                _SelectedChannelId = value;
                var g = _Gps.GetChannelById("RoomMain: Getting Group", value);
                if (g is not null)
                {
                    _SelectedChannel = g;
                }
                var callback = (async () => await _Cme.SelectedChannelChangedEventCallback(g));
                callback.Invoke();
            }
        }
        public ChannelsManager(ILogger<ChannelsManager> logger, GroupDataService gps, AuthorizationManager Am, GroupsManager Gm, GroupManagerEvents gme,ChannelManagerEvents cme)
        {
            _Logger = logger;
            _Gps= gps;
            _Am= Am;
            _Gm = Gm;
            _Gme = gme;
            _Cme = cme;
            _ = InitAsync();
        }
        public async Task InitAsync()
        {
            _Gme.SelectedGroupChangedEvent += OnSelectedGroupChanged;
            CurrentGroup = _Gm.SelectedGroup;
        }
        private async Task OnSelectedGroupChanged(KeyValuePair<Group, bool> newCurrectGroup)
        {
            CurrentGroup = newCurrectGroup;
            _Channels = await _Gps.GetChannelFromGroupAsync("Loading Channels.", CurrentGroup.Key.Id)!;
            _ChannelCategories = await _Gps.GetChannelCatagoryFromGroupAsync("Loading Channel categories List", CurrentGroup.Key.Id)!;
            _ChannelsDictionary =
            await _Gps.GetChannelWithCatagoryFromGroupAsync("Loading Channel map", CurrentGroup.Key.Id);
        }
        public async Task<int> CreateCategory(string categoryName, string uid, int gid)
        {
            if (categoryName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH)
            {
                throw new Exception("Group name is required in StringMarco.MAX_STRING_LENGTH chars");
            }
            StringBuilder log = new StringBuilder();
            log.Append("User：").Append(uid).Append(" ").Append("is creating group ").Append(categoryName);
            var result = _Gps.CreateCategory(log.ToString(), uid, gid, categoryName);
            await OnSelectedGroupChanged(CurrentGroup);
            await _Cme.CateGoryAddedEventCallback(_ChannelsDictionary);
            return result.Entity.Id;
        }
        public async Task<int> CreateChannelAsync(string channelName, string uid, int gid, int cid)
        {
            if (channelName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH)
                throw new Exception("Group name out of bounds");
            StringBuilder log = new StringBuilder();
            log.Append("User：").Append(uid).Append(" ").Append("is creating channel ").Append(channelName);
            var result = _Gps.CreateChannel(log.ToString(), uid, gid, cid, channelName);
            await OnSelectedGroupChanged(CurrentGroup);
            await _Cme.CateGoryAddedEventCallback(_ChannelsDictionary);
            return result.Entity.Id;
        }
        public async Task DeleteCategory(string uid, int cid)
        {
            var sb = new StringBuilder();
            sb.Append(uid).Append(" trying delete channel category ").Append(cid.ToString());
            try
            {
                await _Gps.SetCategoryDeletedAsync(sb.ToString(), cid, uid);
            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
                throw;
            }
        }
        /// <summary>
        /// 更新类别 cid 的名称
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="uid"></param>
        /// <param name="newName"></param>
        /// <returns>Task</returns>
        /// <exception cref="Exception"></exception>
        public async Task UpdateCategoryName(int cid, string uid, string newName)
        {
            var sb = new StringBuilder();
            sb.Append("Category ").Append(cid).Append("updating name: ").Append(newName);
            if (newName.ToASCIIByte().Length >= StringMarco.MAX_STRING_LENGTH || newName.Length == 0) 
                throw new Exception("Group name out of bounds");
            try
            {
                await _Gps.UpdateCategoryNameAsync(sb.ToString(), cid, uid, newName);
            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
                throw;
            }
        }
    }
}
