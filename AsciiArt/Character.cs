using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt
{
    internal class Character
    {
        public int Width { get; private set; }

        public char Ascii { get; private set; }

        private List<string> _Lines;
        public List<string> Lines
        {
            get => _Lines;

            set
            {
                _Lines = value;
                Width = DetermineMaxWidth();
                FontDefinition.Get().MaxCharacterWidth = Math.Max(FontDefinition.Get().MaxCharacterWidth, Width);
            }
        }

        public Character(char ascii)
        {
            Ascii = ascii;
        }

        private int DetermineMaxWidth()
        {
            int result = 0;

            foreach (string line in Lines)
            {
                result = Math.Max(result, line.Length);
            }

            return result;
        }
    
        public void CorrectWhitespaces()
        {
            for (int i = 0; i < _Lines.Count; ++i)
            {
                while(_Lines[i].Length < Width)
                {
                    _Lines[i] += " ";
                }
            }
        }
    }
}
