using AsciiArt.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
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

        public void MoveInline(int cols)
        {
            for (int y = 0; y < _Height; ++y)
            {
                cols = cols % _Width;

                // spalten die wegfallen würden
                char[] temp = new char[cols];
                for (int i = 0; i < cols; i++)
                {
                    temp[i] = _Buffer[y, i];
                }

                // move
                for(int x = 0; x < _Width - cols; ++x)
                {
                    Swap(x, y, x + cols, y);
                }

                // weggefallenes wieder dran machen
                for (int i = 0; i < cols; i++)
                {
                    _Buffer[y, i + (_Width - cols)] = temp[i];
                }
            }
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

        private void Swap(int x1, int y1, int x2, int y2)
        {
            char temp = _Buffer[y1, x1];
            _Buffer[y1, x1] = _Buffer[y2, x2];
            _Buffer[y2, x2] = temp;
        }
    }
}
