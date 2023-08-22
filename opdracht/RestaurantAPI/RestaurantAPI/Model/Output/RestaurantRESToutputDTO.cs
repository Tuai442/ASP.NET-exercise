using RestaurantBL.Models;

namespace RestaurantAPI.Model.Output
{
    public class RestaurantRESToutputDTO
    {
        public RestaurantRESToutputDTO(string restaurantURL, int id, string naam, string keuken,
            string contactGegevens, LocatieRESToutputDTO locatie, List<TafelRESToutputDTO> tafels = null, List<ReservatieRESToutputDTO> reservations = null)
        {
            RestaurantURL = restaurantURL;
            Id = id;
            Naam = naam;
            Keuken = keuken;
            ContactGegevens = contactGegevens;
            Locatie = locatie;
            Tafels = tafels;
            Reservations = reservations;
        }

        public string RestaurantURL { get; }
        public int Id { get; }
        public string Naam { get; }
        public string Keuken { get; }
        public string ContactGegevens { get; }
        public LocatieRESToutputDTO Locatie { get; set; }
        public List<TafelRESToutputDTO> Tafels { get; set; }

        public List<ReservatieRESToutputDTO> Reservations { get; set; }
    }
}