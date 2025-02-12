using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
{
    internal interface ICharacterFactory
    {
        Character Create(char ch);
    }
}
