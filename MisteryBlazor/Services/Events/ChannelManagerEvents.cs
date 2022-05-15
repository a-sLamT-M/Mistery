using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Services.DataManager;

namespace MisteryBlazor.Services.Events
{
    public class ChannelManagerEvents
    {
        private ILogger _Logger;
        public ChannelManagerEvents(ILogger<ChannelManagerEvents> logger)
        {
            _Logger = logger;
        }
        public delegate void CategoryAdded(Dictionary<ChannelCategory, IList<Channel>> map);
        public event CategoryAdded CategoryAddedEvent;
        public delegate Task SelectedChannelChanged(Channel channelSelected);
        public event SelectedChannelChanged SelectedChannelChangedEvent;
        public async Task CateGoryAddedEventCallback(Dictionary<ChannelCategory, IList<Channel>> map)
        {
            if (CategoryAddedEvent is not null)
                CategoryAddedEvent(map);
        }
        public async Task SelectedChannelChangedEventCallback(Channel channelSelected)
        {
            if (SelectedChannelChangedEvent is not null)
                SelectedChannelChangedEvent(channelSelected);
        }
    }
}
