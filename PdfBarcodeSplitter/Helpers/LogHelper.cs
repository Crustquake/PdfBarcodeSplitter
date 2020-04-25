using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

using NLog;
using NLog.Targets;
using NLog.Config;

namespace PdfBarcodeSplitter.Helpers
{
    public class LogHelper
    {
        private static readonly IEnumerable<LogLevel> AllLevels = new[]
        {
            LogLevel.Trace,
            LogLevel.Debug,
            LogLevel.Info,
            LogLevel.Warn,
            LogLevel.Error,
            LogLevel.Fatal,
        };

        public static ILogger TryGetLogger()
        {
            try
            {
                ILogger logger = LogManager.GetCurrentClassLogger();
                return logger;
            }
            catch
            {
                return null;
            }
        }
        public static void ReconfigureLoggerToLevel(LogLevel level)
        {
            if (LogManager.Configuration != null)
            {
                IEnumerable<LogLevel> disableLevels = AllLevels.Where(x => x < level);
                IEnumerable<LogLevel> enableLevels = AllLevels.Where(x => x >= level);
                IEnumerable<LoggingRule> loggingRules = LogManager.Configuration.LoggingRules;

                foreach (LoggingRule rule in loggingRules)
                {
                    foreach (LogLevel lvl in disableLevels)
                    {
                        rule.DisableLoggingForLevel(lvl);
                    }

                    foreach (LogLevel lvl in enableLevels)
                    {
                        rule.EnableLoggingForLevel(lvl);
                    }
                }

                LogManager.ReconfigExistingLoggers();
            }
        }
        public static bool TryReconfigureLoggerToLevel(LogLevel level)
        {
            try
            {
                ReconfigureLoggerToLevel(level);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
