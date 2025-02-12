using AsciiArt.File;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsciiArt
{
    class Program
    {
        private const string _CharacterLocation = @"./Resources";

        private OutputBuffer _Buffer;
        private FileReader _FileReader;

        private ILog _Logger;

        static void Main(string[] args)
        {
            Logger.Configure();
            Program instance = new Program();
            instance.Initialize();
            while (true)
            {
                instance.GetWord();
            }
        }

        public void Initialize()
        {
            _Logger = Logger.GetLogger(typeof(Program).Namespace);
            _FileReader = new FileReader();
            _Buffer = new OutputBuffer(0);
        }

        public void GetWord()
        {
            string word = Console.ReadLine();

            if (string.IsNullOrEmpty(word))
            {
                _Logger.Info($"GetWord - Input was null or empty. Returning to parent routine");
                return;
            }

            _Buffer.Initialize(new char());
            ProcessWord(word);
        }

        private void ProcessWord(string word)
        {
            List<Character> characters = CreateCharacters(word);

            if (characters is null) return;

            int totalLength = FontDefinition.Get().CharacterHeight * FontDefinition.Get().MaxCharacterWidth * characters.Count + FontDefinition.Get().CharacterHeight;
            _Buffer.Resize(totalLength);
            _Buffer.Initialize(new char());

            BufferCharacters(characters);

            _Buffer.Print();
        }

        private List<Character> CreateCharacters(string word)
        {
            List<Character> characters = new List<Character>();
            foreach (char ch in word)
            {
                string filename = $"{_CharacterLocation}/{ch}.txt";
                bool opened = _FileReader.Open(filename);

                if (!opened)
                {
                    Console.WriteLine($"File {filename} could not be opened. Check logs for more information");
                    return null;
                }

                List<string> lines = _FileReader.GetLines().ToList();
                Character character = new Character(ch);
                character.Lines = lines;
                character.CorrectWhitespaces();
                characters.Add(character);
            }
            return characters;
        }

        private void BufferCharacters(List<Character> characters)
        {
            int start = 0;
            int currentLine = 0;
            while (currentLine < FontDefinition.Get().CharacterHeight)
            {
                string line = string.Empty;
                foreach (Character character in characters)
                {
                    line += character.Lines[currentLine];
                }
                _Logger.Debug($"Adding {line} at {start}");
                line += "\n";
                _Buffer.PutLine(line, start);
                start += line.Length;
                ++currentLine;
            }
        }
    }
}
