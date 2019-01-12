using AppUDP.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppUDP
{
    public class ComandoDatabase : IComandoDatabase
    {
        private readonly SQLiteAsyncConnection database;

        public ComandoDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Comando>().Wait();
        }

        public Task<List<Comando>> GetItemsAsync()
        {
            return database.Table<Comando>().ToListAsync();
        }

        public Task<Comando> GetItemAsync(int id)
        {
            return database.Table<Comando>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Comando item)
        {
            if (item.Id != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Comando item)
        {
            return database.DeleteAsync(item);
        }
    }
}