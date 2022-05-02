using MisteryBlazor.Data.User;

namespace MisteryBlazor.Services.MessageServices
{
    public interface IDbService
    {
        Task<List<MisteryIdentityUser>> GetAllUsers(string log);
    }
}
