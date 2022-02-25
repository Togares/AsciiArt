using AsciiArt.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Font
{
    internal class FontDefinition : IFontDefinition
    {
        private IFileReader _FileReader;
        private string _Filename;

        public int TabStop => 4;
        public string FontFolder => @"./_Resources";
        public int CharacterHeight { get; private set; }
        public int MaxCharacterWidth { get; set; }
        public string Characters { get; private set; }

        public FontDefinition(IFileReader fileReader, string filename = "font.def") 
        {
            _FileReader = fileReader;
            _Filename = filename;
            Initialize();
        }

        private void Initialize()
        {
            string path = $"{FontFolder}/{_Filename}";
            bool opened = _FileReader.Open(path);

            if(!opened)
            {
                Console.WriteLine($"File {path} could not be opened. Check logs for more information");
                return;
            }

            string[] lines = _FileReader.GetLines().ToArray();
            CharacterHeight = int.Parse(lines[0]);
            Characters = lines[1];

        }
    }
}
