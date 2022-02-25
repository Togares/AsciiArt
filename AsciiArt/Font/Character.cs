using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
{
    internal class Character
    {
        private IFontDefinition _FontDefinition;

        public int Width { get; private set; }
        public char Ascii { get; private set; }
        public char[,] Matrix { get; private set; }

        private List<string> _Lines;
        public List<string> Lines
        {
            get => _Lines;

            private set
            {
                _Lines = value;
                Width = DetermineMaxWidth();
                CorrectWhitespaces();
                Width = DetermineMaxWidth();
            }
        }

        public Character(char ascii, IFontDefinition fontDefinition)
        {
            Ascii = ascii;
            _FontDefinition = fontDefinition;
        }

        public void CreateMatrix(List<string> lines)
        {
            while(lines.Count > _FontDefinition.CharacterHeight)
            {
                lines.RemoveAt(lines.Count - 1);
            }

            Lines = lines;

            Matrix = new char[_FontDefinition.CharacterHeight, Width];

            for (int y = 0; y < Lines.Count;  ++y)
            {
                for (int x = 0; x < Width; x++)
                {
                    Matrix[y, x] = Lines[y][x];
                }
            }
        }

        private int DetermineMaxWidth()
        {
            int result = 0;

            foreach (string line in Lines)
            {
                result = Math.Max(result, line.Length);
            }

            _FontDefinition.MaxCharacterWidth = Math.Max(_FontDefinition.MaxCharacterWidth, result);

            return result;
        }

        private int CorrectWhitespaces()
        {
            int totalCharsAdded = 0;
            for (int i = 0; i < _Lines.Count; ++i)
            {
                // replace tabs with blanks for consistency
                string line = _Lines[i];
                while (line.Contains("\t"))
                {
                    int tabLocation = line.IndexOf("\t");
                    if (tabLocation >= 0)
                    {
                        int numBlanks = (tabLocation + _FontDefinition.TabStop) / _FontDefinition.TabStop * _FontDefinition.TabStop;
                        string blanks = "";
                        for (int j = tabLocation; j < numBlanks; ++j)
                        {
                            blanks += " ";
                        }

                        _Lines[i] = line = line.Remove(tabLocation, 1);
                        _Lines[i] = line = line.Insert(tabLocation, blanks);

                        totalCharsAdded += numBlanks;
                    }
                }

                // add blanks on the right for consistent width
                while (_Lines[i].Length < Width)
                {
                    _Lines[i] += " ";
                }

            }
            return totalCharsAdded;
        }
    }
}
