using AppUDP.Models;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AppUDP.Service
{
    public class TcpService : ITcpService
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public TcpService()
        {
        }

        private ObservableCollection<Comando> Comandos = new ObservableCollection<Comando>();

        public async Task<string> SendAsync(string ip, int port, string command)
        {
            Comando comm = new Comando(command, ip, port, "TCP");

            string returndata = string.Empty;

            TcpClient tcpClient = new TcpClient();

            try
            {
                await tcpClient.ConnectAsync(ip, port);
            }
            catch (Exception e)
            {
                comm.Receive = e.Message;
            }

            NetworkStream netStream = tcpClient.GetStream();

            if (netStream.CanWrite)
            {
                byte[] sendBytes = Encoding.UTF8.GetBytes(command);
                netStream.Write(sendBytes, 0, sendBytes.Length);
            }
            else
            {
                returndata = "No results.";

                comm.Receive = returndata;

                Comandos.Add(comm);

                tcpClient.Close();

                netStream.Close();
            }

            if (netStream.CanRead)
            {
                byte[] bytes = new byte[tcpClient.ReceiveBufferSize];

                netStream.Read(bytes, 0, tcpClient.ReceiveBufferSize);

                returndata = Encoding.UTF8.GetString(bytes);

                comm.Receive = returndata;
            }
            else
            {
                comm.Receive = "No reponse";

                Comandos.Add(comm);

                tcpClient.Close();

                netStream.Close();
            }

            netStream.Close();

            return returndata;
        }
    }
}