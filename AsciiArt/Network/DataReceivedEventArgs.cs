using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Network
{
    public class DataReceivedEventArgs : IDataReceivedEventArgs
    {
        public DataReceivedEventArgs(string data)
        {
            Data = data;
        }

        public string Data { get; }
    }
}
