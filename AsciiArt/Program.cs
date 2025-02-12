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
            _Buffer = new OutputBuffer(Console.WindowWidth * FontDefinition.Get().CharacterHeight);
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
            int addedChars = 0;
            List<Character> characters = CreateCharacters(word, out addedChars);

            if (characters is null) return;

            int totalLength = FontDefinition.Get().CharacterHeight * FontDefinition.Get().MaxCharacterWidth * characters.Count + FontDefinition.Get().CharacterHeight + addedChars;
            _Buffer.Resize(totalLength);
            _Buffer.Initialize(new char());

            BufferCharacters(characters);

            _Buffer.Print();
        }

        private List<Character> CreateCharacters(string word, out int numAddedChars)
        {
            List<Character> characters = new List<Character>();
            numAddedChars = 0;
            foreach (char ch in word)
            {
                string filename = string.Empty;

                if (ch == ' ') filename = "blank.txt";
                else filename = $"{ch}.txt";

                string path = $"{_CharacterLocation}/{filename}";
                bool opened = _FileReader.Open(path);

                if (!opened)
                {
                    Console.WriteLine($"File {path} could not be opened. Check logs for more information");
                    return null;
                }

                List<string> lines = _FileReader.GetLines().ToList();
                Character character = new Character(ch);
                character.Lines = lines;
                numAddedChars = character.CorrectWhitespaces();
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
