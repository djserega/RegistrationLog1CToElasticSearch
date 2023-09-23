using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace RegistrationLog1CToElasticSearch.EF
{
    public class ReaderContext : DbContext
    {
        private readonly MainConfig _mainConfig;

        public ReaderContext(MainConfig mainConfig)
        {
            _mainConfig = mainConfig;
        }

        public required DbSet<Models.LogModels.AppCodes> AppCodes { get; set; }
        public required DbSet<Models.LogModels.ComputerCodes> ComputerCodes { get; set; }
        public required DbSet<Models.LogModels.EventCodes> EventCodes { get; set; }
        public required DbSet<Models.LogModels.UserCodes> UserCodes { get; set; }
        public required DbSet<Models.LogModels.MetadataCodes> MetadataCodes { get; set; }
        public required DbSet<Models.LogModels.PrimaryPortCodes> PrimaryPortCodes { get; set; }
        public required DbSet<Models.LogModels.EventLog> EventLog { get; set; }

        public async Task<List<Models.LogModels.EventLog>> GetEventLogsAsync(
                Expression<Func<Models.LogModels.EventLog, bool>>? predicate = default,
                Func<IQueryable<Models.LogModels.EventLog>, IOrderedQueryable<Models.LogModels.EventLog>>? orderBy = default,
                int? count = default)
        {
            IQueryable<Models.LogModels.EventLog> query = EventLog;

            if (predicate != null)
                query = query.Where(predicate);

            if (count == default)
            {
                if (orderBy != null)
                    return await orderBy(query).ToListAsync();
                else
                    return await query.ToListAsync();
            }
            else
            {
                if (orderBy != null)
                    return await orderBy(query).Take((int)count!).ToListAsync();
                else
                    return await query.Take((int)count!).ToListAsync();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=" + _mainConfig.SQLiteLogPath);
    }
}
