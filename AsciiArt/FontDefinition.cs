using AsciiArt.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt
{
    class FontDefinition
    {
        private static FontDefinition _Instance;
        
        private const string _Filename = @"./Resources/font.def";

        private FileReader _FileReader = new FileReader();

        public int CharacterHeight { get; private set; }
        public int MaxCharacterWidth { get; set; }

        public string Characters { get; private set; }

        private FontDefinition() 
        {
            Initialize();
        }

        public static FontDefinition Get()
        {
            return _Instance = _Instance == null ? new FontDefinition() : _Instance;
        }

        private void Initialize()
        {
            bool opened = _FileReader.Open(_Filename);

            if(!opened)
            {
                Console.WriteLine($"File {_Filename} could not be opened. Check logs for more information");
                return;
            }

            string[] lines = _FileReader.GetLines().ToArray();
            CharacterHeight = int.Parse(lines[0]);
            Characters = lines[1];

        }
    }
}
