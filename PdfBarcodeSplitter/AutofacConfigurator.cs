using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NLog;
using Autofac;

using PdfBarcodeSplitter.Helpers.LogFolder;
using PdfBarcodeSplitter.Models;
using PdfBarcodeSplitter.ViewModels;

namespace PdfBarcodeSplitter
{
    internal class AutofacConfigurator
    {
        public static IContainer Configure(ILogger logger)
        {
            var builder = new ContainerBuilder();

            builder.Register(context =>
            {
                ILogFolder logFolder = context.Resolve<ILogFolder>();
                IMainModel model = MainModel.Create(logFolder, logger);
                return model;
            }).As<IMainModel>();

            builder.Register(context =>
            {
                IMainModel mainModel = context.Resolve<IMainModel>();
                IMainViewModel mainWindowViewModel = new MainViewModel(mainModel, logger);
                return mainWindowViewModel;
            }).As<IMainViewModel>();

            builder.Register(context =>
            {
                ILogFolder logFolder = new LogFolder();
                return logFolder;
            }).As<ILogFolder>();


            IContainer container = builder.Build();
            return container;
        }
    }
}
