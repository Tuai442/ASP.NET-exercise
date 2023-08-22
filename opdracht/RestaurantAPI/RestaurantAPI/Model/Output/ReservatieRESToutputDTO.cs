namespace RestaurantAPI.Model.Output
{
    public class ReservatieRESToutputDTO
    {
        public ReservatieRESToutputDTO(string reservatieURL, int reservatieNr, Gebruiker gebruiker,
            RestaurantRESToutputDTO restaurant, int tafelNr, int aantalPlaatsen, DateTime datum)
        {
            ReservatieURL = reservatieURL;
            ReservatieNr = reservatieNr;
            Gebruiker = gebruiker;
            
            TafelNr = tafelNr;
            AantalPlaatsen = aantalPlaatsen;
            Datum = datum;

            RestaurantNaam = restaurant.Naam;
            RestaurantContactGegevens = restaurant.ContactGegevens;
            RestaurantLocatie = restaurant.Locatie;
        }
       


        public string ReservatieURL { get; set; }
        public int ReservatieNr { get; set; }
        //public Restaurant Restaurant { get; set; }

        public Gebruiker Gebruiker { get; set; }
        public int AantalPlaatsen { get; set; }
        public DateTime Datum { get; set; }
        public int TafelNr { get; set; }

        public string RestaurantNaam { get; set; }
        public string RestaurantContactGegevens { get; set; }
        public LocatieRESToutputDTO RestaurantLocatie { get; set; }
    }
}