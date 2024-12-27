using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Tournament_421_TyryshkinAD
{
    public partial class App : Application
    {
        private readonly IServiceProvider _provider;

        public App()
        {
            var services = new ServiceCollection();
            
            _provider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {


            base.OnStartup(e);
        }
    }
}
