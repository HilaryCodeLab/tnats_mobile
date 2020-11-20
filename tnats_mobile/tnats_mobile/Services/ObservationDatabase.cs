using SQLite;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// CONSTRUCTOR CLASS
        /// </summary>
        public ObservationDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        /// <summary>
        /// INITIALIZE THE DATABASE
        /// </summary>
        /// <returns></returns>
        async Task InitializeAsync()
        {
            if (!initialized)
            {
                await Database.CreateTablesAsync(CreateFlags.None, typeof(Observation)).ConfigureAwait(false);
                await Database.CreateTablesAsync(CreateFlags.None, typeof(Species)).ConfigureAwait(false);
                await Database.CreateTablesAsync(CreateFlags.None, typeof(Location)).ConfigureAwait(false);
                await Database.CreateTablesAsync(CreateFlags.None, typeof(User)).ConfigureAwait(false);

                initialized = true;
            }
        }

        /// <summary>
        /// OBSERVATION LOCAL DATABASE METHODS
        /// </summary>
        #region Observation
        public Task<List<Observation>> GetItemsAsync()
        {
            return Database.Table<Observation>().ToListAsync();
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

        #endregion

        /// <summary>
        /// SPECIES LOCAL DATABASE METHODS
        /// </summary>
        #region Species
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
        #endregion

        /// <summary>
        /// LOCATIONS LOCAL DATABASE METHODS
        /// </summary>
        #region Locations
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

        #endregion

        /// <summary>
        /// USER LOCAL DATABASE METHODS
        /// </summary>
        #region User

        public Task<List<User>> GetUsers()
        {
            return Database.Table<User>().ToListAsync();
        }
        public Task<User> GetLoggedUser()
        {
            return Database.Table<User>().FirstOrDefaultAsync();
        }
        public Task<int> SaveUser(User item)
        {
            return Database.InsertAsync(item);
        }
         public async void DeleteUser()
        {
            var users = await GetUsers();

            foreach (var item in users)
            {
                await Database.DeleteAsync(item);
            }
        }

        #endregion
    }
}
