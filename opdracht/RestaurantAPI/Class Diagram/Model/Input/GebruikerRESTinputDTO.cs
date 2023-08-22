using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Model.Input
{
    public class GebruikerRESTinputDTO
    {
        public string Naam { get; set; }
        public string Email { get; set; }
        public string TelefoonNr { get; set; }
        public LocatieRESTinputDTO Locatie { get; set; }

        public GebruikerRESTinputDTO(string naam, string email, string telefoonNr, LocatieRESTinputDTO locatie)
        {
            Naam = naam;
            Email = email;
            TelefoonNr = telefoonNr;
            Locatie = locatie;
        }

    }
}