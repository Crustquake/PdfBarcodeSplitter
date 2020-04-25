using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using NLog;

using PdfBarcodeSplitter.Helpers.LogFolder;

namespace PdfBarcodeSplitter.Models
{
    internal class MainModel : IMainModel
    {
        private readonly ILogger _logger;
        private readonly ILogFolder _logFolder;

        #region Creating
        private MainModel(ILogFolder logFolder, ILogger logger)
        {
            _logFolder = logFolder;
            _logger = logger;

            AssemblyName assemblyName = Assembly.GetExecutingAssembly().GetName();
            WindowTitle = assemblyName.Name;
            SoftwareName = $"{assemblyName.Name} {assemblyName.Version.ToString()}";

            //Just test
            Status = "Some status";
        }

        public static MainModel Create(ILogFolder logFolder, ILogger logger)
        {
            return new MainModel(logFolder, logger);
        }
        #endregion

        public string WindowTitle { get; }
        public string Status { get; private set; }
        public string SoftwareName { get; }

        public bool LogFolderExists { get => _logFolder.IsExists; }
        public void OpenLogFolder()
        {
            _logFolder.Open();
        }
    }
}
