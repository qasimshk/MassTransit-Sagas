using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.IO;
using System.Xml;

namespace CQRS.Business.Utils
{
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly string _log4NetConfigFile;

        private readonly ConcurrentDictionary<string, Logger> _loggers =
            new ConcurrentDictionary<string, Logger>();

        public Log4NetProvider(string log4NetConfigFile)
        {
            _log4NetConfigFile = log4NetConfigFile;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        public void Dispose()
        {
            _loggers.Clear();
        }

        private Logger CreateLoggerImplementation(string name)
        {
            return new Logger(name, Parselog4NetConfigFile(_log4NetConfigFile));
        }

        private static XmlElement Parselog4NetConfigFile(string filename)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(filename));
            return log4netConfig["log4net"];
        }
    }
}