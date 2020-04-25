using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PdfBarcodeSplitter.ViewModels
{
    public interface IMainViewModel
    {
        string WindowTitle { get; set; }
        string SoftwareName { get; set; }

        ICommand LogsLinkCommand { get; }
        Visibility LogsLinkVisibility { get; }
    }
}
