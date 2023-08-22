using System.Security.Principal;

namespace RestaurantAPI.Model.Input.Restaurant.Reservatie
{
    public class ReservatieUpdateRESTinputDTO
    {
        //public int ReservatieNr { get; set; }
        public int AantalPlaatsen { get; set; }
        public DateTime Datum { get; set; }

        public ReservatieUpdateRESTinputDTO(int aantalPlaatsen, DateTime datum)
        {
            //ReservatieNr = reservatieNr;
            AantalPlaatsen = aantalPlaatsen;
            Datum = datum;
        }
    }
}