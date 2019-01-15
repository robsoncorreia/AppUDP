using AppUDP.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace AppUDP.Service
{
    public static class UdpService
    {
        public static IList<Comando> Responses { get; set; }

        public delegate void GetRespostas();

        public static event GetRespostas Invertido;

        public static void Send(string ip, string buf, int port = 6666)
        {
            Socket s = new Socket(AddressFamily.InterNetwork,
                                  SocketType.Dgram,
                                  ProtocolType.Udp);

            IPAddress broadcast = IPAddress.Parse(ip);

            byte[] sendbuf = Encoding.ASCII.GetBytes(buf);

            IPEndPoint ep = new IPEndPoint(broadcast, port);

            s.SendTo(sendbuf, ep);

            s.Dispose();
        }

        private static UdpClient listener;

        public static async Task Broadcast(string ip = null, int port = 9999, string comando = "oi", int timer = 1000)
        {

            Responses  = new List<Comando>();

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            CancellationToken token = tokenSource.Token;

            Task t1 = Task.Factory.StartNew(() =>
            {

                listener = new UdpClient(port);

                IPEndPoint groupEP = new IPEndPoint(ip != null ? IPAddress.Parse(ip) : IPAddress.Broadcast, port);

                byte[] bytes2 = Encoding.ASCII.GetBytes(comando);

                listener.Send(bytes2, bytes2.Length, groupEP);

                //client.Close();

                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        Debug.WriteLine("Waiting for broadcast");

                        byte[] bytes = listener.Receive(ref groupEP);

                        Debug.WriteLine($"Received broadcast from {groupEP} :");

                        Debug.WriteLine($" {Encoding.ASCII.GetString(bytes, 0, bytes.Length)}");

                        Responses.Add(new Comando
                        {
                            Receive = Encoding.ASCII.GetString(bytes, 0, bytes.Length),
                            IP = groupEP.Address.ToString(),
                            Port = groupEP.Port
                        });
                    }
                }
                catch (SocketException e)
                {
                    listener.Close();
                    Debug.WriteLine(e);
                }
                finally
                {
                    listener.Close();
                }
            }, token).ContinueWith((t) =>
             {
                 t.Exception.Handle((e) => true);

                 Console.WriteLine("The Task is interrupted");
             },

            TaskContinuationOptions.OnlyOnCanceled);

            await Task.Delay(timer);

            Debug.WriteLine("Fim do Task");

            listener.Close();

            tokenSource.Cancel();

            Invertido();



        }



        public static async Task<string> SendAsync(string ip, int port, string Comando, int tempoEspera)
        {
            string parseIp = IPAddress.Parse(ip).ToString();

            string response = string.Empty;

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            CancellationToken token = tokenSource.Token;

            Send(ip, Comando, port);

            Comando com = new Comando(Comando, ip, port, "UDP");

            Task t1 = Task.Factory.StartNew(() =>
            {
                using (UdpClient listener = new UdpClient(port))
                {
                    IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(parseIp), port);

                    while (!token.IsCancellationRequested)
                    {
                        if (listener?.Available > 0)
                        {
                            listener.EnableBroadcast = true;

                            byte[] bytes = listener.Receive(ref groupEP);

                            Debug.WriteLine($"Received broadcast from {groupEP.ToString()} :\n { Encoding.ASCII.GetString(bytes, 0, bytes.Length)}\n");

                            response = Encoding.ASCII.GetString(bytes, 0, bytes.Length);

                            com.Receive = response;

                            //  Comandos.Add(com);

                            listener.Close();

                            tokenSource.Cancel();
                        }
                    }
                }
            },
            token).ContinueWith((t) =>
            {
                t.Exception.Handle((e) => true);
                Debug.WriteLine("The Task is interrupted");
            }, TaskContinuationOptions.OnlyOnCanceled);

            await Task.Delay(tempoEspera);

            tokenSource.Cancel();



            return response;
        }
    }
}