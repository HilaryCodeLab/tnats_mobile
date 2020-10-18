using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tnats_mobile.Models;
using tnats_mobile.Util;

namespace tnats_mobile.Services
{
    public class ObservationDatabase
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public ObservationDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Observation).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Observation)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        public Task<List<Observation>> GetItemsAsync()
        {
            return Database.Table<Observation>().ToListAsync();
        }

        public Task<List<Observation>> GetItemsNotDoneAsync()
        {
            // SQL queries are also possible
            return Database.QueryAsync<Observation>("SELECT * FROM [Observation]");
        }

        public Task<Observation> GetItemAsync(int id)
        {
            return Database.Table<Observation>().Where(i => i.id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Observation item)
        {
            if (item.id != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Observation item)
        {
            return Database.DeleteAsync(item);
        }
    }
}
