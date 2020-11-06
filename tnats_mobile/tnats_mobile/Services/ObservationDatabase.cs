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
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Species).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Species)).ConfigureAwait(false);
                }
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Location).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Location)).ConfigureAwait(false);
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

        public Task<List<Species>> GetSpecies()
        {
            return Database.Table<Species>().ToListAsync();
        } 
        public Task<int> SaveSpecies(Species item)
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

        public Task<int> DeleteSpecies(Species item)
        {
            return Database.DeleteAsync(item);
        }
        public async void DeleteAllSpecies()
        {
            var list = await GetSpecies();

            foreach (var item in list)
            {
                await Database.DeleteAsync(item);
            }
        }

        public Task<List<Location>> GetLocations()
        {
            return Database.Table<Location>().ToListAsync();
        }
        public Task<int> SaveLocation(Location item)
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

        public Task<int> DeleteLocation(Location item)
        {
            return Database.DeleteAsync(item);
        }
        public async void DeleteAllLocations()
        {
            var list = await GetLocations();

            foreach (var item in list)
            {
                await Database.DeleteAsync(item);
            }
        }
    }
}
