using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Network
{
    public class Server : IServer
    {        
        private TcpListener _Listener;

        private IPEndPoint _EndPoint;

        public int Port { get; set; } = 42069;

        public event DataReceivedHandler DataReceived;

        public Server()
        {
            _EndPoint = new IPEndPoint(IPAddress.Loopback, Port);
        }

        public async Task ListenAsync()
        {
            _Listener = new TcpListener(_EndPoint);
            _Listener.Start();

            while(true)
            {
                using (TcpClient client = await _Listener.AcceptTcpClientAsync())
                {
                    using (NetworkStream stream = client.GetStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string data = await reader.ReadToEndAsync();
                        DataReceived?.Invoke(this, new DataReceivedEventArgs(data));
                    }
                }
            }
        }
    }
}
