using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Network
{
    public delegate void DataReceivedHandler(object sender, IDataReceivedEventArgs args);

    public interface IServer
    {
        event DataReceivedHandler DataReceived;
        Task ListenAsync();
    }
}
