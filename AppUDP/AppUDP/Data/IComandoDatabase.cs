using System.Collections.Generic;
using System.Threading.Tasks;
using AppUDP.Models;

namespace AppUDP
{
    public interface IComandoDatabase
    {
        Task<int> DeleteItemAsync(Comando item);

        Task<Comando> GetItemAsync(int id);

        Task<List<Comando>> GetItemsAsync();

        Task<int> SaveItemAsync(Comando item);
    }
}