using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsciiArtClient
{
    class Client
    {
        static void Main(string[] args)
        {
            Client instance = new Client();

            while (true)
            {
                instance.SendWord();
            }
        }

        public void SendWord()
        {
            byte[] word = Encoding.UTF8.GetBytes(Console.ReadLine());
            
            using (TcpClient client = new TcpClient())
            {
                client.Connect("127.0.0.1", 42069);

                if (client.Connected)
                {
                    client.GetStream().Write(word, 0, word.Length);
                }
            }
        }
    }
}
