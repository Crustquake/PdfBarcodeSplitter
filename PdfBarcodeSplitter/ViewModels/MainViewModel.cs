using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using NLog;

using PdfBarcodeSplitter.Models;
using PdfBarcodeSplitter.ViewModels.Commands;

namespace PdfBarcodeSplitter.ViewModels
{
    internal class MainViewModel : IMainViewModel
    {
        private readonly IMainModel _model;
        private readonly ILogger _logger;

        public MainViewModel(IMainModel model, ILogger logger)
        {
            _model = model;
            _logger = logger;

            WindowTitle = _model.WindowTitle;
            Status = _model.Status;
            SoftwareName = _model.SoftwareName;
            LogsLinkCommand = new SimpleCommand(_model.OpenLogFolder, _model.LogFolderExists);
            LogsLinkVisibility = _model.LogFolderExists ? Visibility.Visible : Visibility.Hidden;
        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region WindowTitle
        private string _windowTitle;

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                if (value != _windowTitle)
                {
                    _windowTitle = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Status
        private string _status;

        public string Status
        {
            get => _status;
            set
            {
                if (value != _status)
                {
                    _status = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region SoftwareName
        private string _softwareName;

        public string SoftwareName
        {
            get => _softwareName;
            set
            {
                if (value != _softwareName)
                {
                    _softwareName = value;
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region LogsLink
        public ICommand LogsLinkCommand { get; }

        public Visibility LogsLinkVisibility { get; }
        #endregion
    }
}
