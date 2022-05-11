using MisteryBlazor.Data.GroupsModel;
using MisteryBlazor.Services.DAL;
using MisteryBlazor.Services.DataManager;

namespace MisteryBlazor.Services.Events
{
    public class GroupManagerEvents
    {
        private ILogger _Logger;
        public GroupManagerEvents(ILogger<GroupManagerEvents> logger)
        {
            _Logger = logger;
        }

        public delegate void GroupAdded(Dictionary<Group, bool> groupsmap, IList<Group> group);
        public event GroupAdded GroupAddedEvent;
        public delegate Task GroupNameUpdated(int id);
        public event GroupNameUpdated GroupNameUpdatedEvent;
        public delegate Task SelectedGroupChanged(KeyValuePair<Group, bool> groupSelected);
        public event SelectedGroupChanged SelectedGroupChangedEvent;

        public async Task SelectedGroupChangedEventCallbackAsync(KeyValuePair<Group, bool> g)
        {
            await SelectedGroupChangedEvent(g);
        }
        public async Task GroupNameUpdatedEventCallbackAsync(int id)
        {
            await GroupNameUpdatedEvent(id);
        }
        public async Task GroupAddedEventCallbackAsync(Dictionary<Group, bool> groupsmap, IList<Group> group)
        {
            GroupAddedEvent(groupsmap, group);
        }
    }
}
