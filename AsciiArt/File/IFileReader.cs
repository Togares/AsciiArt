using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.File
{
    internal interface IFileReader
    {
        bool Open(string filename);

        IEnumerable<string> GetLines();
    }
}
