using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using NLog;
using Autofac;

using PdfBarcodeSplitter.Helpers;
using PdfBarcodeSplitter.ViewModels;
using PdfBarcodeSplitter.Models;
using PdfBarcodeSplitter.Views;

namespace PdfBarcodeSplitter
{
    internal class Client
    {
        private readonly UnhandledExceptionEventHandler _currentDomainOnUnhandledExceptionEventHandler;
        private readonly EventHandler<UnobservedTaskExceptionEventArgs> _taskSchedulerOnUnobservedTaskExceptionEventHandler;
        private readonly DispatcherUnhandledExceptionEventHandler _dispatcherOnUnhandledExceptionEventHandler;
        private readonly ExitEventHandler _applicationExitEventHandler;
        private readonly EventHandler _mainWindowClosedEventHandler;

        private readonly ILogger _logger;

        public Client()
        {
            _currentDomainOnUnhandledExceptionEventHandler = (sender, args) => CurrentDomainOnUnhandledExceptionEventHandler(sender, args);
            _dispatcherOnUnhandledExceptionEventHandler = (sender, args) => DispatcherOnUnhandledExceptionEventHandler(sender, args);
            _taskSchedulerOnUnobservedTaskExceptionEventHandler = (sender, args) => TaskSchedulerOnUnobservedTaskExceptionEventHandler(sender, args);
            _applicationExitEventHandler = (sender, args) => ApplicationExitEventHandler(sender, args);
            _mainWindowClosedEventHandler = (sender, args) => MainWindowClosedEventHandler(sender, args);

            _logger = LogHelper.TryGetLogger();
        }

        public void Run()
        {
            try
            {
#if DEBUG
                LogHelper.TryReconfigureLoggerToLevel(LogLevel.Debug);
#endif
                IContainer autofacContiner = AutofacConfigurator.Configure(_logger);

                using (ILifetimeScope scope = autofacContiner.BeginLifetimeScope())
                {
                    IMainViewModel mainWindowViewModel = scope.Resolve<IMainViewModel>();
                    IMainModel mainWindowModel = scope.Resolve<IMainModel>(TypedParameter.From(mainWindowViewModel));
                    Window mainWindow = new MainWindow(mainWindowViewModel);
                    Application application = new Application();

                    AppDomain.CurrentDomain.UnhandledException += _currentDomainOnUnhandledExceptionEventHandler;
                    TaskScheduler.UnobservedTaskException += _taskSchedulerOnUnobservedTaskExceptionEventHandler;
                    application.Dispatcher.UnhandledException += _dispatcherOnUnhandledExceptionEventHandler;
                    application.Exit += _applicationExitEventHandler;
                    mainWindow.Closed += _mainWindowClosedEventHandler;

                    application.Run(mainWindow);
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void HandleException(Exception exception)
        {
            if (_logger != null)
            {
                _logger.Fatal(exception.ToString());
                MessageBox.Show($"Fatal error with reason: {exception.Message}. Please look log file",
                    "Fatal error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show($"Fatal error with reason: {exception.ToString()}.",
                    "Fatal error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        private void CurrentDomainOnUnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs args)
        {
            HandleException(args.ExceptionObject as Exception);
        }
        private void TaskSchedulerOnUnobservedTaskExceptionEventHandler(object sender, UnobservedTaskExceptionEventArgs args)
        {
            args.SetObserved();
            HandleException(args.Exception.GetBaseException());
        }
        private void DispatcherOnUnhandledExceptionEventHandler(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            HandleException(args.Exception);
        }
        private void ApplicationExitEventHandler(object sender, ExitEventArgs args)
        {
            _logger?.Info("Application exit");
            LogManager.Flush();
        }
        private void MainWindowClosedEventHandler(object sender, EventArgs args)
        {
            _logger?.Info("Main window closed");
        }
    }
}
