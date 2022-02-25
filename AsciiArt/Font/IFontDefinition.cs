using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
{
    internal interface IFontDefinition
    {
        int TabStop { get; }
        string FontFolder { get; }
        int CharacterHeight { get; }
        int MaxCharacterWidth { get; set; }
    }
}
