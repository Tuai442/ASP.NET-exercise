namespace RestaurantAPI.Model.Output
{
    public class TafelListRESToutputDTO
    {
        public int Id { get; set; }
        public int Plaatsen { get; set; }
        public string TafelURL { get; set; }
        public TafelListRESToutputDTO(int id, string tafelURL, int plaatsen)
        {
            Id = id;
            Plaatsen = plaatsen;
            TafelURL = tafelURL;
        }
    }
}