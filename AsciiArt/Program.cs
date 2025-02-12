using AsciiArt.File;
using AsciiArt.Font;
using AsciiArt.Logging;
using AsciiArt.Network;
using CommunicationModel;
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
        private ILog _Logger;
        private IServer _Server;
        private IFileReader _FileReader;
        private IFontDefinition _FontDefinition;
        private ICharacterFactory _CharacterFactory;

        private volatile ClientData _ClientData;

        private OutputBuffer _Buffer;
        private List<Character> _CurrentWord;

        public static bool Moving = false;

        static void Main(string[] args)
        {
            Logger.Configure();
            Program instance = new Program();
            instance.Initialize();

            while (true)
            {
                if (Moving)
                {
                    instance.MoveWord();
                }
            }
        }

        public void Initialize()
        {
            _Logger = Logger.GetLogger(typeof(Program).Namespace);

            _FileReader = new FileReader();
            _FontDefinition = new FontDefinition(_FileReader);
            _CharacterFactory = new CharacterFactory(_FileReader, _FontDefinition);

            _Server = new Server();
            _Server.DataReceived += OnDataReceived;
            _ = _Server.ListenAsync();
        }

        public void MoveWord()
        {
            _Buffer.MoveInline(_ClientData.Speed);
            Thread.Sleep(_ClientData.Interval);
            Console.Clear();
            _Buffer.Print();
        }

        private void OnDataReceived(object sender, IDataReceivedEventArgs args)
        {
            if (args is null || string.IsNullOrEmpty(args.Data))
            {
                _Logger.Info($"GetWord - Input was null or empty. Returning to parent routine");
                return;
            }

            ProcessData(args);
            _Buffer.Print();
            Moving = !string.IsNullOrEmpty(args.Data);
        }

        private void ProcessData(IDataReceivedEventArgs args)
        {
            _ClientData = CommunicationData.Deserialize(args.Data);

            _CurrentWord = CreateCharacters(_ClientData.Data);

            if (_CurrentWord is null) return;

            int width = Console.WindowWidth;
            int height = _FontDefinition.CharacterHeight;

            _Buffer = new OutputBuffer(height, width);

            _Buffer.FillWith(' ');
            BufferCharacters(_CurrentWord);
        }

        private List<Character> CreateCharacters(string word)
        {
            List<Character> characters = new List<Character>();
            foreach (char ch in word)
            {
                Character character = _CharacterFactory.Create(ch);
                if(character != null) characters.Add(character);
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
