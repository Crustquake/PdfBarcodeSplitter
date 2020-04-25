using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using NLog;
using NLog.Targets;

namespace PdfBarcodeSplitter.Helpers.LogFolder
{
    internal class LogFolder : ILogFolder
    {
        private readonly string _logFolderPath;

        public LogFolder()
        {
            try
            {
                _logFolderPath = GetLogFolderPath();

                if (!IsExists)
                {
                    Directory.CreateDirectory(_logFolderPath);
                }
            }
            catch
            {
            }
        }

        public bool IsExists
        {
            get
            {
                if (string.IsNullOrEmpty(_logFolderPath))
                    return false;

                return Directory.Exists(_logFolderPath);
            }
        }
        public void Open()
        {
            if (IsExists)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(_logFolderPath)
                {
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            else
            {
                throw new ApplicationException("Log folder doesn't exist");
            }
        }

        private string GetLogFolderPath()
        {
            FileTarget logFileTarget = GetFileTarget();
            if (logFileTarget == null)
                throw new ApplicationException("Exception during find log configuration");

            string logFilePath = logFileTarget.FileName.Render(new LogEventInfo { Level = LogLevel.Info });
            string logFolderPath = Path.GetDirectoryName(logFilePath.ToString());
            return logFolderPath;
        }
        private FileTarget GetFileTarget()
        {
            FileTarget fileTarget = LogManager.Configuration.AllTargets.FirstOrDefault(t => t is FileTarget) as FileTarget;
            return fileTarget;
        }
    }
}
