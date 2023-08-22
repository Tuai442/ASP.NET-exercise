using RestaurantAPI.Model.Input.Restaurant.Tafel;

namespace RestaurantAPI.Model.Input.Restaurant
{
    public class RestaurantUpdateRESTinputDTO
    {
        public string Naam { get; set; }
        public LocatieRESTinputDTO Locatie { get; set; }
        public string Keuken { get; set; }
        public string Telefoon { get; set; }
        public string Email { get; set; }
        public List<TafelUpdateRESTinputDTO> Tafels { get; set; } = null;

        public RestaurantUpdateRESTinputDTO(string naam, LocatieRESTinputDTO locatie, string keuken,
            string telefoon, string email, List<TafelUpdateRESTinputDTO> tafels = null)
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
