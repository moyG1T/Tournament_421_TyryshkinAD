using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using Tournament_421_TyryshkinAD.Data;
using Tournament_421_TyryshkinAD.Domain.Contexts;
using Tournament_421_TyryshkinAD.Domain.IServices;
using Tournament_421_TyryshkinAD.Domain.Services;
using Tournament_421_TyryshkinAD.ViewModels;

namespace Tournament_421_TyryshkinAD
{
    public partial class App : Application
    {
        private readonly IServiceProvider _provider;

        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<DbEntities>();
            services.AddSingleton<MainContext>();

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainViewModel>(p =>
            {
                return new MainViewModel(
                    BackOnlyFactory(p),
                    PlayerFactory(p),
                    BackOnlyFactory(p),
                    p.GetRequiredService<MainContext>()
                    );
            });

            services.AddTransient<PlayerViewModel>(p =>
            {
                return new PlayerViewModel(p.GetRequiredService<DbEntities>());
            });

            _provider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = _provider.GetRequiredService<MainWindow>();
            var viewModel = _provider.GetRequiredService<MainViewModel>();
            MainWindow.DataContext = viewModel;
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected MainNavService PlayerFactory(IServiceProvider p)
        {
            return new MainNavService(p.GetRequiredService<MainContext>(), p.GetRequiredService<PlayerViewModel>);
        }
        protected MainNavService BackOnlyFactory(IServiceProvider p)
        {
            return new MainNavService(p.GetRequiredService<MainContext>());
        }
    }
}
