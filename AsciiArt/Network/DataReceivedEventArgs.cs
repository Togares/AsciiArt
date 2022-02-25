using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Network
{
    public class DataReceivedEventArgs
    {
        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }
}
