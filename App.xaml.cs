using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Res.Models;
using Res.Exceptions;
using Res.ViewModels;
using Res.Stores;
using Res.Services;
using Microsoft.EntityFrameworkCore;
using Res.DbContexts;
using Res.Services.ReservationProviders;
using Res.Services.ReservationCreators;
using Res.Services.ReservationConflictValidators;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Res.HostBuilders;

namespace Res
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        

        private readonly IHost _host;

   
        public App()

        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                 .ConfigureServices((hostContext,services) =>
                { 
                 string connectionString = hostContext.Configuration.GetConnectionString("Default");
                 

                 services.AddSingleton<ResDbContextFactory>(new ResDbContextFactory(connectionString));
                 services.AddSingleton<IReservationProvider, DatabaseReservationProvider>();
                 services.AddSingleton<IReservationCreator, DatabaseReservationCreator>();
                 services.AddSingleton<IReservationConflictValidator, DatabaseReservationConflictValidator>();

                 services.AddTransient<ReservationBook>();
                 services.AddSingleton((s) => new Hotel("ASD Suites", s.GetRequiredService<ReservationBook>()));

                

                 services.AddSingleton<HotelStore>();
                 services.AddSingleton<NavigationStore>();

                 
                 services.AddSingleton(s => new MainWindow()
                 {
                     DataContext = s.GetRequiredService<MainViewModel>()
                 });

            })
                .Build();


           
        }

        

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            ResDbContextFactory resDbContextFactory = _host.Services.GetRequiredService<ResDbContextFactory>();
            using (ResDbContext dbContext = resDbContextFactory.CreateDbContext())
            { 
                dbContext.Database.Migrate(); 
            }

            NavigationService<ReservationListingViewModel> navigationService = _host.Services.GetRequiredService<NavigationService<ReservationListingViewModel>>();
            navigationService.Navigate();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();

            base.OnExit(e);
        }


    }
}
