using Res.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel; 
using Res.Commands;
using Res.Stores;
using Res.Services;

namespace Res.ViewModels
{
    public class ReservationListingViewModel : ViewModelBase
    {
        private readonly ObservableCollection<ReservationViewModel> _reservations;

        public IEnumerable<ReservationViewModel> Reservations => _reservations;

        private string _errorMessage;

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
           
        }
        public MakeReservationViewModel MakeReservationViewModel { get; }

        private bool _isLoading;

        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }

            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }


        private readonly HotelStore _hotelStore;
        public ICommand LoadReservationCommand { get; }
        public ICommand MakeReservationCommand { get; }

        public ReservationListingViewModel( HotelStore hotelStore, MakeReservationViewModel makeReservationViewModel, NavigationService makeReservationNavigationService)
        {
            _hotelStore = hotelStore;
            _reservations = new ObservableCollection<ReservationViewModel>();
            MakeReservationViewModel = makeReservationViewModel;
            LoadReservationCommand = new LoadReservationsCommand(this, hotelStore);
            MakeReservationCommand = new NavigateCommand(makeReservationNavigationService);

            _hotelStore.ReservationMade += OnReservationMade;
        }


        ~ReservationListingViewModel()
        {

        }

        public override void Dispose() 
        {
            _hotelStore.ReservationMade -= OnReservationMade;
            base.Dispose();
        }


        private void OnReservationMade(Reservation reservation)
        {
            ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
            _reservations.Add(reservationViewModel);
        }

        public static ReservationListingViewModel LoadViewModel(HotelStore hotelStore, 
            MakeReservationViewModel makeReservationViewModel,
            NavigationService makeReservationNavigationSerivce)
        {
            ReservationListingViewModel viewModel = new ReservationListingViewModel(hotelStore, makeReservationViewModel, makeReservationNavigationSerivce);
            
            viewModel.LoadReservationCommand.Execute(null);
            return viewModel;
        }

        public void UpdateReservations(IEnumerable<Reservation> reservations)
        {
            _reservations.Clear();

            foreach (Reservation reservation in reservations)
            {
                ReservationViewModel reservationViewModel = new ReservationViewModel(reservation);
                _reservations.Add(reservationViewModel);
            }
        }
    }
}
