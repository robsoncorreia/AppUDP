using System.Threading.Tasks;

namespace AppUDP.Service
{
    public interface ITcpService
    {
        Task<string> SendAsync(string ip, int port, string command);
    }
}