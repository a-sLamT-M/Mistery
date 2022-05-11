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
        public delegate Task CategoryUpdated();
        public event CategoryUpdated CategoryUpdateEvent;

        public async Task CateGoryUpdatedEventCallback()
        {
            if(CategoryUpdateEvent is not null)
                _ = CategoryUpdateEvent();
        }
    }
}
