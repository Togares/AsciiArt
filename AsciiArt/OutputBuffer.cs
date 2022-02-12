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

        private char[] _Buffer;
        public int Length => _Buffer.Length;

        private int _ResizeDelta;

        public int ResizeDelta => _ResizeDelta;

        private int _RotationPerFrame;

        private int _LineLength;

        public OutputBuffer(int size)
        {
            _Buffer = new char[size];
            Initialize(new char());
        }

        /// <summary>
        /// Initializes the buffer with a given character
        /// </summary>
        /// <param name="fill">The character to fill the buffer with</param>
        public void Initialize(char fill)
        {
            for (int i = 0; i < Length; i++)
            {
                _Buffer[i] = fill;
            }
        }

        public void PutLine(string line, int where)
        {
            if (line is null) throw new ArgumentNullException(nameof(line));

            if (line.Length + where > Length) throw new ArgumentOutOfRangeException($"{line} would exceed buffers size, if put at {where}");

            for (int i = where, j = 0; j < line.Length; i++, j++)
            {
                _Buffer[i] = line[j];
            }
            _LineLength = Math.Max(line.Length, _LineLength);
        }

        public void Resize(int length)
        {
            _ResizeDelta = Math.Abs(length - Length);
            _RotationPerFrame = _ResizeDelta / FontDefinition.Get().MaxCharacterWidth;
            _Buffer = new char[length];
            _Logger.Info($"Resized Buffer from {length - _Buffer.Length} to {length}");
        }

        public override string ToString()
        {
            string result = string.Empty;

            for (int i = 0; i < _Buffer.Length; i++)            
            {
                result += _Buffer[i];
            }

            return result;
        }

        public void Print()
        {
            Console.WriteLine(ToString());
            using(var writer = new StreamWriter(@"./test.txt"))
            {
                writer.Write(ToString());
            }
        }
    }
}
