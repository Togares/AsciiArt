using CommunicationModel;
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

            string input = Console.ReadLine();

            ClientData data = ParseInput(input);

            byte[] word = Encoding.UTF8.GetBytes(CommunicationData.Serialize(data));
            
            using (TcpClient client = new TcpClient())
            {
                client.Connect("127.0.0.1", 42069);

                if (client.Connected)
                {
                    client.GetStream().Write(word, 0, word.Length);
                }
            }
        }

        private ClientData ParseInput(string input)
        {
            ClientData result = new ClientData();

            string[] data = input.Split('\u002C');

            try
            {
                result.Data = data[0];
            }
            catch (Exception)
            {
                result.Data = "";
            }

            try
            {
                result.Interval = int.Parse(data[1]);
            }
            catch (Exception)
            {
                result.Interval = 500;
            }
            result.IntervalSpecified = true;

            try
            {
                result.Speed = int.Parse(data[2]);
            }
            catch (Exception)
            {
                result.Speed = 1;
            }
            result.SpeedSpecified = true;

            return result;
        }
    }
}
