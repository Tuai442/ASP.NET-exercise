using RestaurantBL.Models;

namespace RestaurantAPI.Model.Output
{
    public class RestaurantListRESToutputDTO
    {
        
        public string Id { get; set; }
        public string Naam { get; set; }

        public List<TafelRESToutputDTO> Tafels { get; set; }
        public List<ReservatieRESToutputDTO> Reservaties { get; set; }
        public RestaurantListRESToutputDTO(string id, string naam, List<TafelRESToutputDTO> tafels=null,
            List<ReservatieRESToutputDTO> reservaties = null)
        {
            Id = id;
            Naam = naam;
            Tafels = tafels;
            Reservaties = reservaties;
        }
    }
}