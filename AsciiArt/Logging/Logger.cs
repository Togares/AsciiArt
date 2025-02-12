using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsciiArt.Logging
{
    internal class Logger
    {
        public static void Configure()
        {
            XmlConfigurator.Configure(new FileInfo(@"./log4net.xml"));
        }

        public static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }
    }
}
