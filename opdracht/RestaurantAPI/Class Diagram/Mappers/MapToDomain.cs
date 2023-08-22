using RestaurantAPI.Model.Input;
using RestaurantAPI.Model.Input.Restaurant;
using RestaurantAPI.Model.Input.Restaurant.Reservatie;
using RestaurantAPI.Model.Input.Restaurant.Tafel;
using RestaurantBL;
using RestaurantBL.Models;
using RestaurantBL.Services;
using System.Net.Mail;

namespace AdresbeheerREST.Mappers
{
    public static class MapToDomain
    {
      
        public static Gebruiker MapToGebruikerDomain(int klantNr, GebruikerRESTinputDTO gebruikerDTO, GebruikerService gebruikerService)
        {
            try
            {

                Locatie locatie = MapToDomain.MapToLocatieDomain(gebruikerDTO.Locatie);
                locatie.ZetId(gebruikerService.GeefLocatieId(klantNr));
                Gebruiker gebruiker = new Gebruiker(klantNr, gebruikerDTO.Naam, gebruikerDTO.Email, gebruikerDTO.TelefoonNr, locatie);
                
                return gebruiker;
            
            }
            catch (Exception ex)
            {
                
                throw new MapException(ex.Message);
            }
        }

        public static Gebruiker MapToGebruikerDomain(GebruikerRESTinputDTO gebruikerDTO)
        {
            try
            {

                Locatie locatie = MapToDomain.MapToLocatieDomain(gebruikerDTO.Locatie);
                Gebruiker gebruiker = new Gebruiker(gebruikerDTO.Naam, gebruikerDTO.Email, gebruikerDTO.TelefoonNr, locatie);

                return gebruiker;

            }
            catch (Exception ex)
            {

                throw new MapException(ex.Message);
            }
        }

        public static Reservatie MapToReservatieDomain(ReservatieRESTinputDTO reservatieDTO,
            RestaurantService restaurantService, GebruikerService gebruikerService)
        {
            // TODO - voorbeeld hoe het niet moet
            try
            {
                Restaurant restaurant = restaurantService.GeefRestaurant(reservatieDTO.RestaurantId);
                Gebruiker gebruiker = gebruikerService.GeefGebruiker(reservatieDTO.KlantNr);

                if (restaurant == null || gebruiker == null)
                {
                    throw new MapException("Gebruiker, Restaurant bestaat niet. ");
                }

                Tafel tafel = restaurant.GeefVrijeTafel(0, reservatieDTO.Datum, reservatieDTO.AantalPlaatsen);
                if (tafel == null)
                {
                    throw new MapException("Geen tafel beschikbaar");
                }

                Reservatie reservatie = new Reservatie(gebruiker, restaurant, 
                    reservatieDTO.AantalPlaatsen, reservatieDTO.Datum, tafel); 

                restaurant.VoegReservatieToe(reservatie);

                return reservatie;

            }
            catch (Exception ex)
            {
                throw new MapException("MapToReservatieDomain", ex);
            }
        }

        public static Reservatie MapToReservatieUpdateDomain(int id, ReservatieUpdateRESTinputDTO restDTO, 
            ReservatieService reservatieService)
        {
            try
            {
                Reservatie reservatie = reservatieService.GeefReservatie(id);
                reservatie.Datum = restDTO.Datum;
                reservatie.AantalPlaatsen = restDTO.AantalPlaatsen;
                return reservatie;

            }
            catch (Exception ex)
            {
                throw new MapException("MapToRestaurantDomain", ex);
            }
        }
        public static Restaurant MapToRestaurantDomain(int id, RestaurantRESTinputDTO restaurantDTO,
            RestaurantService _restaurantService)
        {
            try
            {
                Restaurant restaurant = null;
                Locatie locatie = MapToLocatieDomain(restaurantDTO.Locatie);
                locatie.ZetId(_restaurantService.GeefLocatieId(id));
                List<Tafel> tafels = new List<Tafel>();
                if (restaurantDTO.Tafels != null && restaurantDTO.Tafels.Count > 0)
                {
                    foreach(var tafel in restaurantDTO.Tafels)
                    {
                        tafels.Add(MapToTafelDomain(tafel));
                    }
                }

                restaurant = new Restaurant(id, restaurantDTO.Naam, locatie, restaurantDTO.Keuken,
                    restaurantDTO.Telefoon, restaurantDTO.Email, tafels);
                
                return restaurant;

            }
            catch (Exception ex)
            {
                throw new MapException("MapException", ex) ;
            }
        }

        public static Restaurant MapToRestaurantDomain(RestaurantRESTinputDTO restaurantDTO)
        {
            try
            {
                Restaurant restaurant = null;
                Locatie locatie = MapToLocatieDomain(restaurantDTO.Locatie);
                
                List<Tafel> tafels = new List<Tafel>();
                if (restaurantDTO.Tafels != null && restaurantDTO.Tafels.Count > 0)
                {
                    foreach (var tafel in restaurantDTO.Tafels)
                    {
                        tafels.Add(MapToTafelDomain(tafel));
                    }
                }

                restaurant = new Restaurant(restaurantDTO.Naam, locatie, restaurantDTO.Keuken,
                    restaurantDTO.Telefoon, restaurantDTO.Email, tafels);

                return restaurant;

            }
            catch (Exception ex)
            {
                string message = ExceptionHandler.GetMessage(ex);
                throw new MapException(message);
            }
        }

        public static Locatie MapToLocatieDomain(LocatieRESTinputDTO locatie)
        {
            try
            {
                return new Locatie(locatie.PostCode, locatie.Gemeente, locatie.Straat, locatie.HuisNr);
            }
            catch (Exception ex)
            {
                throw new MapException("MapToLocatieDomain", ex);
            }
        }

        public static Tafel MapToTafelUpdateDomain(TafelUpdateRESTinputDTO tafelDTO)
        {
            try
            {
                Tafel? tafel = null;
                
                tafel = new Tafel(tafelDTO.Id, tafelDTO.Plaatsen);

                return tafel;

            }
            catch (Exception ex)
            {
                throw new MapException("MapToRestaurantDomain", ex);
            }
        }

        public static Tafel MapToTafelDomain(TafelRESTinputDTO tafelDTO)
        {
            try
            {
                Tafel? tafel = null;

                tafel = new Tafel(tafelDTO.Plaatsen);

                return tafel;

            }
            catch (Exception ex)
            {
                throw new MapException("MapToRestaurantDomain", ex);
            }
        }

        internal static Restaurant MapToRestaurantUpdateDomain(int id, RestaurantUpdateRESTinputDTO restDTO)
        {
            try
            {
                Restaurant restaurant = null;
                Locatie locatie = MapToLocatieDomain(restDTO.Locatie);

                List<Tafel> tafels = new List<Tafel>();
                if (restDTO.Tafels != null && restDTO.Tafels.Count > 0)
                {
                    foreach (var tafel in restDTO.Tafels)
                    {
                        tafels.Add(MapToTafelUpdateDomain(tafel));
                    }
                }

                restaurant = new Restaurant(id, restDTO.Naam, locatie, restDTO.Keuken,
                    restDTO.Telefoon, restDTO.Email, tafels);

                return restaurant;

            }
            catch (Exception ex)
            {
                string message = ExceptionHandler.GetMessage(ex);
                throw new MapException(message);
            }
        }
    }
}
