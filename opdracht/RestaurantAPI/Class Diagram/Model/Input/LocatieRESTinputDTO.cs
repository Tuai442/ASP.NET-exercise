using System.Text.RegularExpressions;

namespace RestaurantAPI.Model.Input
{
    public class LocatieRESTinputDTO
    {
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

        public LocatieRESTinputDTO(string postCode, string gemeente, string straat, int huisNr)
        {
            PostCode = postCode;
            Gemeente = gemeente;
            Straat = straat;
            HuisNr = huisNr;
        }
    }
}
