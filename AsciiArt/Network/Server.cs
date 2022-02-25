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

        public int Port { get; set; } = 42069;
        public IPAddress IP { get; private set; }        

        public event DataReceivedHandler DataReceived;

        public Server()
        {            
            IP = Dns.GetHostEntry("127.0.0.1").AddressList[0];
        }

        public async Task ListenAsync()
        {
            _Listener = new TcpListener(IP, Port);
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
