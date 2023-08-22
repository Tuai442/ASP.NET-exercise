

using System;
using System.Reflection.Emit;
using RestaurantAPI.Mappers;
using RestaurantAPI.Model.Input;
using RestaurantAPI.Model.Output;
using RestaurantBL.Models;
using RestaurantBL.Services;

namespace AdresbeheerREST.Mappers
{
    public static class MapFromDomain
    {

        public static GebruikerRESToutputDTO MapFromGebruikerDomain(string hostURL, Gebruiker gebruiker)
        {
            try
            {

                LocatieRESToutputDTO locatie = MapFromDomain.MapFromLocatieDomain(hostURL, gebruiker.Locatie);
                string gebruikerURL = $"{hostURL}/gebruiker/{gebruiker.KlantNr}";
                GebruikerRESToutputDTO dto = new GebruikerRESToutputDTO(gebruikerURL, gebruiker.KlantNr, gebruiker.Naam,
                    gebruiker.Email, gebruiker.TelefoonNr, locatie);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromGebruikerDomain", ex);
            }

        }

        private static LocatieRESToutputDTO MapFromLocatieDomain(string hostURL, Locatie locatie)
        {
            try
            {
                string locatieUrl = $"{hostURL}/locatie/{locatie.Id}";
                LocatieRESToutputDTO dto = new LocatieRESToutputDTO(locatieUrl, locatie.Id, locatie.PostCode, locatie.Gemeente, locatie.Straat, locatie.HuisNr ) ;
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromLocatieDomain", ex);
            }
        }

        internal static ReservatieRESToutputDTO MapFromReservatieDomain(string hostURL, Reservatie reservatie)
        {
            try
            {
                RestaurantRESToutputDTO restaurantDTO = MapFromDomain.MapFromRestaurantDomain(
                    hostURL, reservatie.Restaurant);

                string reservatieURL = $"{hostURL}/reservatie/{reservatie.ReservatieNr}";
                ReservatieRESToutputDTO dto = new ReservatieRESToutputDTO(reservatieURL, reservatie.ReservatieNr,
                    reservatie.Gebruiker, restaurantDTO, reservatie.Tafel.Id, reservatie.AantalPlaatsen, reservatie.Datum);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromReservatieDomain", ex);
            }
        }

        internal static ReservatieRESToutputDTO MapFromReservatieDomain(string hostURL, Reservatie reservatie,
           RestaurantRESToutputDTO restaurantDTO)
        {
            try
            {
                

                string reservatieURL = $"{hostURL}/reservatie/{reservatie.ReservatieNr}";
                ReservatieRESToutputDTO dto = new ReservatieRESToutputDTO(reservatieURL, reservatie.ReservatieNr,
                    reservatie.Gebruiker, restaurantDTO, reservatie.Tafel.Id, reservatie.AantalPlaatsen, reservatie.Datum);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromReservatieDomain", ex);
            }
        }

        internal static RestaurantRESToutputDTO MapFromRestaurantDomain(string hostURL, Restaurant restaurant)
        {
            try
            {

                string restaurantURL = $"{hostURL}/restaurant/{restaurant.Id}";

                List<TafelRESToutputDTO> tafels = new List<TafelRESToutputDTO>();
                List<ReservatieRESToutputDTO> reservaties = new List<ReservatieRESToutputDTO>();
                if(restaurant.Tafels != null)
                {
                    foreach (Tafel tafel in restaurant.Tafels)
                    {

                        tafels.Add(MapFromTafelDomain(restaurantURL, tafel));
                    }
                }
               

                LocatieRESToutputDTO locatie = MapFromLocatieDomain(restaurantURL, restaurant.Locatie);
                RestaurantRESToutputDTO dto = new RestaurantRESToutputDTO(restaurantURL, restaurant.Id, restaurant.Naam, 
                    restaurant.Keuken, restaurant.ContactGegevens, locatie, tafels, reservaties);

                foreach (Reservatie reservatie in restaurant.Reservaties)
                {
                    reservaties.Add(MapFromReservatieDomain(restaurantURL, reservatie, dto));
                }
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }

        internal static TafelRESToutputDTO MapFromTafelDomain(string hostURL, Tafel tafel)
        {
            try
            {

                string tafelURL = $"{hostURL}/tafel/{tafel.Id}";
                TafelRESToutputDTO dto = new TafelRESToutputDTO(tafel.Id, tafelURL, tafel.Plaatsen);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }
    }
}
