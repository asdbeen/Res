using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Res.Exceptions;
using Res.Services.ReservationConflictValidators;
using Res.Services.ReservationCreators;
using Res.Services.ReservationProviders;

namespace Res.Models
{
    public class ReservationBook
    {


        private readonly IReservationProvider _reservationProvider;
        private readonly IReservationCreator _reservationCreator;
        private readonly IReservationConflictValidator _reservationConflictValidator;

        public ReservationBook(IReservationProvider reservationProvider, IReservationCreator reservationCreator, IReservationConflictValidator reservationConflictValidator)
        {
            _reservationProvider = reservationProvider;
            _reservationCreator = reservationCreator;
            _reservationConflictValidator = reservationConflictValidator;
        }

        /// <summary>
        /// Get all reservation.
        /// </summary>
        /// <returns>All reservation in the reservation book.</returns>
        public async Task<IEnumerable<Reservation>> GetAllReservations()
        {
            return await _reservationProvider.GetAllReservations();
        }

        public async Task AddReservation(Reservation reservation)
        {
            Reservation conflictingReservation = await _reservationConflictValidator.GetConflictReservation(reservation);

            if(conflictingReservation != null)
            {
                throw new ReservationConflictException(conflictingReservation, reservation);
            }

          
            await  _reservationCreator.CreateReservation(reservation);
        }
    }
}
