using System;
using System.Collections.Generic;
using System.Text;

namespace PdfBarcodeSplitter.Helpers.LogFolder
{
    internal interface ILogFolder
    {
        bool IsExists { get; }
        void Open();
    }
}
