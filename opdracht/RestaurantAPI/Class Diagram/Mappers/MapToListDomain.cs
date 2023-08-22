using AdresbeheerREST.Mappers;
using RestaurantAPI.Model.Input.Restaurant.Tafel;
using RestaurantAPI.Model.Output;
using RestaurantBL.Models;
using System.Reflection.Emit;

namespace RestaurantAPI.Mappers
{
    public class MapToListDomain
    {
        internal static List<Tafel> MapToTafelDomain(List<TafelListRESTinputDTO> tafels)
        {
            try
            {
                List<Tafel> result = new List<Tafel>();
                foreach (var t in tafels)
                {
                    result.Add(new Tafel(t.Plaatsen));

                }
                return result;
            }
            catch (Exception ex)
            {
                throw new MapException("MapFromRestaurantDomain", ex);
            }
        }
    }
}
