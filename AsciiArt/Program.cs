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

        private List<Character> _CurrentWord;

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
        }

        public void GetWord()
        {
            string word = Console.ReadLine();
            Console.Clear();

            if (string.IsNullOrEmpty(word))
            {
                _Logger.Info($"GetWord - Input was null or empty. Returning to parent routine");
                return;
            }

            ProcessWord(word);
            _Buffer.Print();
        }

        private void ProcessWord(string word)
        {
            _CurrentWord = CreateCharacters(word);

            if (_CurrentWord is null) return;

            int width = Console.WindowWidth; /*FontDefinition.Get().MaxCharacterWidth* _CurrentWord.Count*/
            int height = FontDefinition.Get().CharacterHeight;
            
            _Buffer = new OutputBuffer(height, width);

            _Buffer.FillWith(' ');
            BufferCharacters(_CurrentWord);
        }

        private List<Character> CreateCharacters(string word)
        {
            List<Character> characters = new List<Character>();
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
                character.CreateMatrix(lines);                
                characters.Add(character);
            }
            return characters;
        }

        private void BufferCharacters(List<Character> characters)
        {
            foreach (var c in characters)
            {
                _Buffer.PutCharacter(c);
            }
        }
    }
}
