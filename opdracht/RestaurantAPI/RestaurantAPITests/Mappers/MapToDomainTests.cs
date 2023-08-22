using Xunit;
using AdresbeheerREST.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantAPI.Model.Input;
using RestaurantBL.Services;
using RestaurantBL.Interfaces;
using RestaurantDL.Repositories;
using RestaurantBL.Models;

namespace AdresbeheerREST.Mappers.Tests
{
    public class MapToDomainTests
    {
        private string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\Tuur\Documents\Gegevens\BACK_UP_LAPTOP\hogent\Web 4\opdracht\RestaurantAPI\Database\Database.mdf"";Integrated Security=True";


        [Theory]
        [InlineData("", "Van Holen", "0499460142", "Gent", 0)]
        [InlineData("Tuur", "", "0499460142", "Gent", 0)]
        [InlineData("Tuur", "Van Holen", "000", "Gent", 0)]
        public void MapToGebruikerDomainTest(string naam, string email, string telefoonNr, string locatie, int klantNr)
        {
            //var dto = new GebruikerRESTinputDTO(naam, email, telefoonNr, locatie, klantNr);

           // Assert.Throws<MapException>(() => MapToDomain.MapToGebruikerDomain(dto));
        }

        [Theory]
        [InlineData(1, 0, 1, 10, "2022-12-25", 4)]
        [InlineData(5, 5, 1, 10, "2022-12-11", 4)]
        public void MapToReservatieDomainTest(int reservatieNr, int restaurantId, int klantNr, int aantalPlaatsen,
            string d, int tafelId)
        {
            DateTime datum = Convert.ToDateTime(d);

            //IReservatieRepository reservatieRepo = new ReservatieRepositoryADO(_connectionString);
            IRestaurantRepository restaurantRepo = new RestaurantRepositoryADO(_connectionString);
            IGebruikerRepository gebruikerRepository = new GebruikerRepositoryADO(_connectionString);
            //ReservatieService reservatieService = new ReservatieService(reservatieRepo, restaurantRepo);
            RestaurantService restaurantSer = new RestaurantService(restaurantRepo);
            GebruikerService gebruikerSer = new GebruikerService(gebruikerRepository);

            ReservatieRESTinputDTO dto = new ReservatieRESTinputDTO(reservatieNr, restaurantId, klantNr, aantalPlaatsen, datum, tafelId);
            Assert.Throws<MapException>(() => MapToDomain.MapToReservatieDomain(dto, restaurantSer, gebruikerSer));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void MapToRestaurantDomainTest(RestaurantRESTinputDTO restaurant)
        {
            var a = "d";
            Assert.Throws<MapException>(() => MapToDomain.MapToRestaurantDomain(restaurant));

        }

        public static IEnumerable<RestaurantRESTinputDTO> Data =>
        new List<RestaurantRESTinputDTO>()
        {
           // new RestaurantRESTinputDTO("", "gent", "keuken", "contact geg.")
               
        };
    }
}