using RestaurantAPI.Model.Input.Restaurant.Tafel;
using RestaurantBL.Models;

namespace RestaurantAPI.Model.Input.Restaurant
{
    public class RestaurantRESTinputDTO
    {
        public string Naam { get; set; }
        public LocatieRESTinputDTO Locatie { get; set; }
        public string Keuken { get; set; }
        public string Telefoon { get; set; }
        public string Email { get; set; }
        public List<TafelRESTinputDTO> Tafels { get; set; } = null;

        public RestaurantRESTinputDTO(string naam, LocatieRESTinputDTO locatie, string keuken,
            string telefoon, string email, List<TafelRESTinputDTO> tafels = null)
        {
            Naam = naam;
            Locatie = locatie;
            Keuken = keuken;
            Telefoon = telefoon;
            Email = email;
            Tafels = tafels;
        }
    }
}