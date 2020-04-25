using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfBarcodeSplitter.Models
{
    internal interface IMainModel
    {
        string WindowTitle { get; }
        string Status { get; }
        string SoftwareName { get; }

        bool LogFolderExists { get; }
        void OpenLogFolder();
    }
}
