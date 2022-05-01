using MisteryBlazor.Data.Context;

namespace MisteryBlazor.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly AppDbContext _context;

        private MessageService(AppDbContext context)
        {

        }
    }
}
