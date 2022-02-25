using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
{
    internal interface ICharacterBuffer
    {
        void PutCharacter(Character character);
        void MoveInline(int cols);
    }
}
