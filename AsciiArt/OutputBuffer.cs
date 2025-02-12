using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt
{
    internal class OutputBuffer
    {
        private ILog _Logger = Logger.GetLogger(typeof(OutputBuffer).Namespace);
        private char[,] _Buffer;
        private int _Width;
        private int _Height;
        private List<Character> _ContainedChars = new List<Character>();

        public OutputBuffer(int lines, int cols)
        {
            _Height = lines;
            _Width = cols;
            _Buffer = new char[lines, cols];
        }

        public void PutCharacter(Character character)
        {
            int startCol = AccumulateCharacterWidth();

            for (int y = 0; y < _Height; y++)
            {
                for (int xBuff = startCol, xChar = 0; xBuff < _Width && xChar < character.Width; ++xBuff, ++xChar)
                {
                    _Buffer[y, xBuff] = character.Matrix[y, xChar];
                }
            }

            _ContainedChars.Add(character);
        }

        public void FillWith(char c)
        {
            for (int y = 0; y < _Height; ++y)
            {
                for (int x = 0; x < _Width; ++x)
                {
                    _Buffer[y, x] = c;
                }
            }
        }

        public void Print()
        {
            for (int y = 0; y < _Height; ++y)
            {
                for (int x = 0; x < _Width; ++x)
                {
                    Console.Write(_Buffer[y, x]);
                }
            }
        }

        private int AccumulateCharacterWidth()
        {
            int result = 0;
            foreach (Character character in _ContainedChars)
            {
                result += character.Width;
            }
            return result;
        }
    }
}
