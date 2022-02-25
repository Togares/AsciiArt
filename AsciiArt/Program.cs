﻿using AsciiArt.File;
using AsciiArt.Font;
using AsciiArt.Logging;
using AsciiArt.Network;
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
        private IFileReader _FileReader;
        private IFontDefinition _FontDefinition;
        private ICharacterFactory _CharacterFactory;

        private OutputBuffer _Buffer;
        private List<Character> _CurrentWord;
        private Server _Server;


        public static bool Moving = false;

        static void Main(string[] args)
        {
            Logger.Configure();
            Program instance = new Program();
            instance.Initialize();

            while (true)
            {
                while (Moving)
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
            _ = _Server.Start();
        }

        public void MoveWord()
        {
            _Buffer.MoveInline(1);
            Thread.Sleep(200);
            Console.Clear();
            _Buffer.Print();
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs args)
        {
            string word = args.Data;
            if (string.IsNullOrEmpty(word))
            {
                _Logger.Info($"GetWord - Input was null or empty. Returning to parent routine");
                return;
            }

            ProcessWord(word);
            _Buffer.Print();
            Moving = true;
        }

        private void ProcessWord(string word)
        {
            _CurrentWord = CreateCharacters(word);

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
