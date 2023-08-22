namespace RestaurantAPI.Model.Output
{
    public class GebruikerRESToutputDTO
    {
        public string GebruikerURL { get; set; }
        public string Naam { get; set; }
        public string Email { get; set; }
        public string TelefoonNr { get; set; }
        public LocatieRESToutputDTO Locatie { get; set; }
        public int KlantNr { get; set; }

        public GebruikerRESToutputDTO(string gebruikerURL, int klantNr, string naam, string email, string telefoonNr, LocatieRESToutputDTO locatie)
        {
            GebruikerURL = gebruikerURL;
            KlantNr = klantNr;
            Naam = naam;
            Email = email;
            TelefoonNr = telefoonNr;
            Locatie = locatie;
        }
    }
}