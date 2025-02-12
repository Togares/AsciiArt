using AsciiArt.File;
using AsciiArt.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
{
    internal class CharacterFactory : ICharacterFactory
    {
        private ILog _Logger = Logger.GetLogger(typeof(CharacterFactory).Namespace);
        private IFileReader _FileReader;
        private IFontDefinition _FontDefinition;

        public CharacterFactory(IFileReader fileReader, IFontDefinition fontDefinition)
        {
            _FileReader = fileReader;
            _FontDefinition = fontDefinition;
        }

        public Character Create(char ch)
        {
            string filename = string.Empty;

            if (ch == ' ') filename = "blank.txt";
            else filename = $"{ch}.txt";

            string path = $"{_FontDefinition.FontFolder}/{filename}";
            bool opened = _FileReader.Open(path);

            if (!opened)
            {
                string error = $"File {path} could not be opened. Check logs for more information";
                _Logger.Error(error);
                Console.WriteLine(error);
                return null;
            }

            List<string> lines = _FileReader.GetLines().ToList();
            Character character = new Character(ch, _FontDefinition);
            character.CreateMatrix(lines);
            return character;
        }
    }
}
