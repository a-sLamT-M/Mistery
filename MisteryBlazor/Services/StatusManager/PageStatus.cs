namespace MisteryBlazor.StatusManager
{
    public class PageStatus
    {
        private ILogger _Logger;

        public PageStatus(ILogger<PageStatus> logger)
        {
            _Logger = logger;
        }
    }
}
