using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace MFG_DigitalApp.Log
{
    public class Logger: ILogger
    {
        private const string LocalFileTarget = "MFG_DigitalAppLog";

        private readonly NLog.Logger _logger;


        private Logger(string clsname)
        {
            if (LogManager.Configuration == null || (LogManager.Configuration != null && !LogManager.Configuration.AllTargets.Any()))
                InitializeTargetsForLoggers();
            _logger = LogManager.GetLogger(clsname);
        }

        public static ILogger GetLogger(string clsname)
        {
            return new Logger(clsname);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(message, exception, null);
        }

        public void Error(Exception exception)
        {
            _logger.Error(exception);
        }

        private void InitializeTargetsForLoggers()
        {
            TargetWithLayout target = new FileTarget() //We are running Local 
            {
                Layout = Layout.FromString(ConfigurationManager.AppSettings["LogFormat"]),
                Name = LocalFileTarget,
                FileName = ConfigurationManager.AppSettings["LogLocation"]
            };
            var loglevel = (LogLevel) Convert.ToInt16(ConfigurationManager.AppSettings["LogLevel"]);
            LoggingConfiguration config = new LoggingConfiguration(); //LogManager.Configuration;
            config.AddTarget(target.Name, target);
            config.LoggingRules.Add(new LoggingRule("*", NLogMethod(loglevel), target));
            LogManager.Configuration = config;
            LogManager.ReconfigExistingLoggers();
        }

        public static void ReconfigureLogLevel(LogLevel loglevel)
        {
            LogManager.GlobalThreshold = NLogMethod(loglevel);
        }

        public static LogLevel GetCurrentLogLevel()
        {
            return NLog2Method(LogManager.GlobalThreshold);
        }

        private static NLog.LogLevel NLogMethod(LogLevel loglevel)
        {
            NLog.LogLevel level;
            switch (loglevel)
            {
                case LogLevel.Trace:
                    level = NLog.LogLevel.Trace;
                    break;
                case LogLevel.Debug:
                    level = NLog.LogLevel.Debug;
                    break;
                case LogLevel.Info:
                    level = NLog.LogLevel.Info;
                    break;
                case LogLevel.Warn:
                    level = NLog.LogLevel.Warn;
                    break;
                case LogLevel.Error:
                    level = NLog.LogLevel.Error;
                    break;
                case LogLevel.Fatal:
                    level = NLog.LogLevel.Fatal;
                    break;
                default:
                    level = NLog.LogLevel.Info;
                    break;
            }
            return level;
        }

        private static LogLevel NLog2Method(NLog.LogLevel loglevel)
        {
            LogLevel level = LogLevel.Off;
            if (loglevel == NLog.LogLevel.Trace)
                level = LogLevel.Trace;
            else if (loglevel == NLog.LogLevel.Debug)
                level = LogLevel.Debug;
            else if (loglevel == NLog.LogLevel.Info)
                level = LogLevel.Info;
            else if (loglevel == NLog.LogLevel.Warn)
                level = LogLevel.Warn;
            else if (loglevel == NLog.LogLevel.Error)
                level = LogLevel.Error;
            else if (loglevel == NLog.LogLevel.Fatal)
                level = LogLevel.Fatal;
            return level;
        }

        public enum LogLevel
        {
            Trace,
            Debug,
            Info,
            Warn,
            Error,
            Fatal,
            Off
        }
    }
}
