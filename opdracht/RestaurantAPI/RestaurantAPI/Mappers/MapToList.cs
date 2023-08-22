using AdresbeheerREST.Mappers;
using RestaurantAPI.Model.Input;
using RestaurantAPI.Model.Output;
using RestaurantBL.Models;
using RestaurantBL.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace RestaurantAPI.Mappers
{
    internal class MapToList
    {
        internal static RestaurantListRESToutputDTO MapToListRestaurant(string hostURL, Restaurant restaurant)
        {
            List< TafelRESToutputDTO > tafels = new List< TafelRESToutputDTO >();
            List< ReservatieRESToutputDTO > res = new List<ReservatieRESToutputDTO>();
            if (restaurant.Tafels.Count > 0)
            {
                foreach(var tafel in restaurant.Tafels)
                {
                    tafels.Add(MapFromTafelDomain(hostURL, tafel));
                }
                foreach(Reservatie reservatie in restaurant.Reservaties)
                {
                    RestaurantRESToutputDTO restaurantDTO = MapFromDomain.MapFromRestaurantDomain(hostURL, restaurant);
                    res.Add(new ReservatieRESToutputDTO(hostURL, reservatie.ReservatieNr, reservatie.Gebruiker, restaurantDTO,
                        reservatie.Tafel.Id, reservatie.AantalPlaatsen, reservatie.Datum));
                }
                
            }
            return new RestaurantListRESToutputDTO($"{hostURL}/{restaurant.Id}", restaurant.Naam, tafels, res);
        }

        internal static TafelRESToutputDTO MapFromTafelDomain(string hostURL, Tafel tafel)
        {
            try
            {

                string tafelURL = $"{hostURL}/restaurant/{tafel.Id}";

                TafelRESToutputDTO dto = new TafelRESToutputDTO(tafel.Id, tafelURL,  tafel.Plaatsen);
                return dto;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }

        internal static TafelListRESToutputDTO MapToListTafels(string hostURL, Tafel tafel)
        {
            try
            {
                string tafelURL = $"{hostURL}/restaurant/{tafel.Id}";
                TafelListRESToutputDTO tafelDTO =  new TafelListRESToutputDTO(tafel.Id, tafelURL, tafel.Plaatsen);

                return tafelDTO;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }
    }
}