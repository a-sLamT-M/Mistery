using Microsoft.EntityFrameworkCore;
using MisteryBlazor.Data.Context;

namespace MisteryBlazor.Data.Seeder
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    public class Seeder
    {
        private AppDbContext DbContext;
        private ILogger _Logger;

        public Seeder(IServiceProvider service, ILogger logger)
        {
            this.DbContext = new AppDbContext(service.GetRequiredService<DbContextOptions<AppDbContext>>());
            this._Logger = logger;
        }

        public async Task Seed()
        {
            try
            {

            }
            catch (Exception e)
            {
                _Logger.LogError(e.Message);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
