using AsciiArt.Logging;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.File
{
    internal class FileReader : IFileReader
    {
        private StreamReader _Reader;
        private ILog _Logger;

        public FileReader()
        {            
            _Logger = Logger.GetLogger(typeof(FileReader).Namespace);
        }

        public bool Open(string filename)
        {
            bool result = false;
            try
            {
                _Reader = new StreamReader(filename);
                result = true;
            }
            catch (Exception e)
            {
                _Logger.Error(e);
            }
            return result;
        }

        public IEnumerable<string> GetLines()
        {
            while(!_Reader.EndOfStream)
            {                
                yield return _Reader.ReadLine();
            }
            _Reader.Dispose();
            _Reader.Close();
        }
    }
}
