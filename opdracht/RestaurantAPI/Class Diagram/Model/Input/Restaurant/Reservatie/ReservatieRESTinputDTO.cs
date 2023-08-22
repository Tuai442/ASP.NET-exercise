using RestaurantAPI.Model.Output;

namespace RestaurantAPI.Model.Input.Restaurant.Reservatie
{
    public class ReservatieRESTinputDTO
    {

        public int RestaurantId { get; set; }
        public int KlantNr { get; set; }
        public int AantalPlaatsen { get; set; }
        public DateTime Datum { get; set; }
        //public int TafelId { get; set; }

        public ReservatieRESTinputDTO(int restaurantId, int klantNr, int aantalPlaatsen, DateTime datum)
        {
            RestaurantId = restaurantId;
            KlantNr = klantNr;
            AantalPlaatsen = aantalPlaatsen;
            Datum = datum;
            //TafelId = tafelId;
        }
    }
}