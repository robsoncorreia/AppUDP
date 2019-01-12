using AppUDP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppUDP.Service
{
    public interface IUdpService
    {
        List<Comando> Responses { get; set; }

        Task Broadcast(int port = 9999, string comando = "oi", int timer = 1000);

        void Send(string ip, string buf, int port = 6666);

        Task<string> SendAsync(string ip, int port, string comando);
    }
}