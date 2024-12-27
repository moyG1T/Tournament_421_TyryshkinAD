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
            services.AddSingleton<TourContext>();

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
                return new PlayerViewModel(CreateTeamFactory(p), SelectTourFactory(p), p.GetRequiredService<DbEntities>());
            });
            services.AddTransient<CreateTeamViewModel>(p =>
            {
                return new CreateTeamViewModel(CreateTeamFactory(p), p.GetRequiredService<DbEntities>());
            });
            services.AddTransient<SelectTourViewModel>(p =>
            {
                return new SelectTourViewModel(TourFactory(p), p.GetRequiredService<TourContext>(), p.GetRequiredService<DbEntities>());
            });
            services.AddTransient<TourViewModel>(p =>
            {
                return new TourViewModel(BackOnlyFactory(p), p.GetRequiredService<TourContext>(), p.GetRequiredService<DbEntities>());
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
        protected MainNavService CreateTeamFactory(IServiceProvider p)
        {
            return new MainNavService(p.GetRequiredService<MainContext>(), p.GetRequiredService<CreateTeamViewModel>);
        }
        protected MainNavService SelectTourFactory(IServiceProvider p)
        {
            return new MainNavService(p.GetRequiredService<MainContext>(), p.GetRequiredService<SelectTourViewModel>);
        }
        protected MainNavService TourFactory(IServiceProvider p)
        {
            return new MainNavService(p.GetRequiredService<MainContext>(), p.GetRequiredService<TourViewModel>);
        }
        protected MainNavService BackOnlyFactory(IServiceProvider p)
        {
            return new MainNavService(p.GetRequiredService<MainContext>());
        }
    }
}
