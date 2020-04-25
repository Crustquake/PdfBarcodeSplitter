using System;
using System.Windows;

using PdfBarcodeSplitter.ViewModels;

namespace PdfBarcodeSplitter.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
