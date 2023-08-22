using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RestaurantBL.Models
{
    public class Controleer
    {
        public static void Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new GebruikerException("ControleEmail");
            string regexString = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex regex = new Regex(regexString);
            if (!regex.IsMatch(email)) throw new GebruikerException("ControleEmail - geen geldige regex");
        }

        public static void Telefoon(string telefoon)
        {
            if (string.IsNullOrWhiteSpace(telefoon)) throw new GebruikerException("ControleTelefoon");
            string regexString = @"^(((\+32|0|0032)4){1}[1-9]{1}[0-9]{7})$";
            Regex regex = new Regex(regexString);
            if (!regex.IsMatch(telefoon)) throw new GebruikerException("ControleTelefoon - geen geldige regex");
        }

        internal static void PostCode(string postCode)
        {
            string regexString = "^[0-9]{4}$";
            Regex regex = new Regex(regexString);
            if (!regex.IsMatch(postCode)) throw new LocatieExcepiton("Postcode is niet correct");
        }
    }
}
