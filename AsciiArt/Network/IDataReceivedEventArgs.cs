using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Network
{
    public interface IDataReceivedEventArgs
    {
        string Data { get; }
    }
}
