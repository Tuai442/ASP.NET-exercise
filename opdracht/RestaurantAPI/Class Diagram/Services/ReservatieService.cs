using System;
using RestaurantBL.Exceptions;
using RestaurantBL.Interfaces;
using RestaurantBL.Models;

namespace RestaurantBL.Services
{
    public class ReservatieService
    {
        private IReservatieRepository _reservatieRepo;
        private IRestaurantRepository _restaurantRepo;

        public ReservatieService(IReservatieRepository reservatieRepo, IRestaurantRepository restaurantRepo)
        {
            _reservatieRepo = reservatieRepo;
            _restaurantRepo = restaurantRepo;

        }

        public Reservatie GeefReservatie(int id)
        {
            try
            {
                return _reservatieRepo.GeefReservatie(id);
            }
            catch (Exception ex)
            {
                throw new ReservatieServiceException("GeefReservatie", ex);
            }
        }


        public Reservatie VoegReservatieToe(Restaurant restaurant, Gebruiker gebruiker, int aantalPlaatsen, DateTime datum)
        {
            try
            {

                if (datum < DateTime.Now) throw new ReservatieServiceException("Je kan geen reservatie in het verleden maken");
                Tafel tafel = GeefVrijeTafel(0, restaurant, aantalPlaatsen, datum);
                Reservatie reservatie = new Reservatie(gebruiker, restaurant, aantalPlaatsen, datum, tafel);

                restaurant.VoegReservatieToe(reservatie);
                _reservatieRepo.VoegReservatieToe(reservatie);
                return reservatie;
            }
            catch (ReservatieServiceException) { throw; }
            catch (ReservatieException) { throw; }
            catch (Exception ex)
            {
                throw new ReservatieException(ex.Message);
            }

        }

        private static Tafel GeefVrijeTafel(int reservatieNr, Restaurant restaurant, int aantalPlaatsen, DateTime datum)
        {
            Tafel tafel = restaurant.GeefVrijeTafel(reservatieNr, datum, aantalPlaatsen);
            if (tafel == null) throw new ReservatieServiceException("Er zijn geen tafels beschikbaar");
            return tafel;
        }

        public bool BestaatReservatie(int reservatieNr)
        {
            try
            {
                return _reservatieRepo.HeeftReservatie(reservatieNr);
            }
            catch (Exception ex)
            {
                throw new ReservatieServiceException("BestaatReservatie", ex);
            }
        }

        public void VerwijderReservatie(int id)
        {
            try
            {
                if (!_reservatieRepo.HeeftReservatie(id)) throw new ReservatieServiceException("VerwijderReservatie - reservatie bestaat niet");
                Reservatie reservatie = _reservatieRepo.GeefReservatie(id);
                if (reservatie.Datum < DateTime.Now) throw new ReservatieServiceException("VerwijderReservatie - je kan geeg reservatie in het verleden verwijderen");
                _reservatieRepo.VerwijderReservatie(reservatie);
            }
            catch (ReservatieServiceException) { throw; }
            catch (Exception ex)
            {
                throw new RestaurantServiceException("Verwijderrestaurant", ex);
            }
        }

        public Reservatie UpdateReservatie(int reservatieNr, int aantalPlaatsen, DateTime datum)
        {
            try
            {
                if (!_reservatieRepo.HeeftReservatie(reservatieNr)) throw new ReservatieServiceException("UpdateReservatie - bestaat niet");
                Reservatie reservatie = _reservatieRepo.GeefReservatie(reservatieNr);
                if (reservatie == null) throw new ReservatieServiceException("UpdateReservatie - null");

                Restaurant restaurant = _restaurantRepo.GeefRestaurant(reservatie.Restaurant.Id);
                Tafel tafel = GeefVrijeTafel(reservatieNr, restaurant, aantalPlaatsen, datum);
                reservatie.Tafel = tafel;
                reservatie.AantalPlaatsen = aantalPlaatsen;
                reservatie.Datum = datum;

                _reservatieRepo.UpdateReservatie(reservatie);
                return reservatie;
            }
            catch (ReservatieServiceException) { throw; }
            catch (ReservatieException) { throw; }
            catch (Exception ex)
            {

                throw new ReservatieServiceException($"ReseravatieService", ex);
            }
        }

        public List<Reservatie> GeefReservatieOpDatum(DateTime beginDatum, DateTime eindDatum)
        {
            try
            {
                return _reservatieRepo.GeefReservatieOpDatum(beginDatum, eindDatum);
            }
            
            catch(Exception ex)
            {
                throw new ReservatieServiceException("GeefReservatieOpDatum", ex);
            }
        }
    }

}
