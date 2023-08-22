namespace RestaurantAPI.Model.Output
{
    public class LocatieRESToutputDTO
    {
        public string HostURL { get; set; }
        public int Id { get; set; }
        public string PostCode
        {
            get; set;
        }

        public string Gemeente
        {
            get; set;
        }
        public string Straat { get; set; }

        public int HuisNr { get; set; }

        public LocatieRESToutputDTO(string hostUrl, int id, string postCode, string gemeente, string straat, int huisNr)
        {
            HostURL = hostUrl;
            Id = id; ;
            PostCode = postCode;
            Gemeente = gemeente;
            Straat = straat;
            HuisNr = huisNr;
        }
    }
}