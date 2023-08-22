using Xunit;
using RestaurantAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantAPI.Model.Input;
using RestaurantBL.Services;

namespace RestaurantAPI.Controllers.Tests
{
   

    public class RestaurantControllerTests
    {

        private ReservatieService _reservatieService;
        private RestaurantService _restaurantService;
        private GebruikerService _gebruikerService;

        private RestaurantController _controller;

        public RestaurantControllerTests()
        {
            _controller = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService);
        }

        [Theory]
        [InlineData(0, 10, "2022-12-14")]

        public void PutReservatieTest(int reservatieNr, int aantalPlaatsen, string datum)
        {
            DateTime date = DateTime.Parse(datum);
            var dto = new ReservatieUpdateRESTinputDTO(reservatieNr, aantalPlaatsen, date);
            _controller.PutReservatie(reservatieNr, dto);
        }
    }
}