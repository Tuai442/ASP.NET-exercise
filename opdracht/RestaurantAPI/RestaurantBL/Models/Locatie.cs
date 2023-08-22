using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestaurantBL.Models
{
    public class Locatie
    {
        private string _postCode;
        private string _gemeente;

        public string PostCode
        {
            get => _postCode;
            set
            {
                Controleer.PostCode(value);
                _postCode = value;
            }
        }

        public string Gemeente { get => _gemeente;
            set 
            {
                if (string.IsNullOrEmpty(value)) throw new LocatieExcepiton("Gemeente mag niet leeg zijn.");
                _gemeente = value;
            }
        }
        public string Straat { get; set; }

        public int HuisNr { get; set; }
        public int Id { get; set; }

        public Locatie(string postCode, string gemeente, string straat, int huisNr)
        {
           
            PostCode = postCode;
            Gemeente = gemeente;
            Straat = straat;
            HuisNr = huisNr;
        }

        public Locatie(int id, string postCode, string gemeente, string straat, int huisNr)
        {
            Id = id;
            PostCode = postCode;
            Gemeente = gemeente;
            Straat = straat;
            HuisNr = huisNr;
        }

        public void ZetId(int id)
        {
            Id = id;
        }
    }
}
