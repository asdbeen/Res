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

namespace Res
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string CONNECTION_STRING = "Data Source = res.db";
        private readonly Hotel _hotel;
        private readonly HotelStore _hotelStore;
        private readonly NavigationStore _navigationStore;
        private readonly ResDbContextFactory _resDbContextFactory;
        public App()

        {
            _resDbContextFactory = new ResDbContextFactory(CONNECTION_STRING);
            IReservationProvider reservationProvider = new DatabaseReservationProvider(_resDbContextFactory);
            IReservationCreator reservationCreator = new DatabaseReservationCreator(_resDbContextFactory);
            IReservationConflictValidator reservationConflictValidator = new DatabaseReservationConflictValidator(_resDbContextFactory);



            ReservationBook reservationBook = new ReservationBook(reservationProvider,reservationCreator,reservationConflictValidator);
            _hotel = new Hotel("ASD Suites",reservationBook);
            _hotelStore = new HotelStore(_hotel);
            _navigationStore = new NavigationStore();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(CONNECTION_STRING).Options;
            using (ResDbContext dbContext = new ResDbContext(options))
            { dbContext.Database.Migrate(); }
            

            _navigationStore.CurrentViewModel = CreateReservationViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };

            MainWindow.Show();

            base.OnStartup(e);
        }

        private MakeReservationViewModel CreateMakeReservationViewModel()
        {
            return new MakeReservationViewModel(_hotelStore, new NavigationService(_navigationStore, CreateReservationViewModel));
        }

        private ReservationListingViewModel CreateReservationViewModel()
        {
            return ReservationListingViewModel.LoadViewModel(_hotelStore,CreateMakeReservationViewModel(),new NavigationService(_navigationStore, CreateMakeReservationViewModel));
        }

    }
}
