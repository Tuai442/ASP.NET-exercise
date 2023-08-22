namespace RestaurantAPI.Model.Input.Restaurant.Tafel
{
    public class TafelListRESTinputDTO
    {
        public int Plaatsen { get; set; }

        public TafelListRESTinputDTO(int plaatsen)
        {
            Plaatsen = plaatsen;
        }
    }
}
