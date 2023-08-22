namespace RestaurantAPI.Model.Output
{
    public class TafelRESToutputDTO
    {
        public int Id { get; set; }
        public int Plaatsen { get; set; }
        public string TafelURL { get; }


        public TafelRESToutputDTO(int id, string tafelURL, int plaatsen)
        {
            Id = id;
            TafelURL = tafelURL;
            Plaatsen = plaatsen;
        }
    }
}
