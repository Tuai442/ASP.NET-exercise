using RestaurantBL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantBL.Interfaces
{
    public  interface ILocatieRepository
    {
        public void VoeglocatieToe(Locatie locatie);
    }
}
