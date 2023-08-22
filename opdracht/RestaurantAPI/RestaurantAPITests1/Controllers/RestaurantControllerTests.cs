using Xunit;
using RestaurantAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using RestaurantBL.Interfaces;
using RestaurantBL.Services;
using RestaurantAPI.Model.Input.Restaurant;
using RestaurantAPI.Model.Input;
using AdresbeheerREST.Mappers;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Model.Output;
using RestaurantBL.Models;
using RestaurantAPI.Model.Input.Restaurant.Tafel;
using Xunit.Sdk;
using RestaurantAPI.Model.Input.Restaurant.Reservatie;
using Castle.Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace RestaurantAPI.Controllers.Tests
{
    public class RestaurantControllerTests
    {

        private Mock<IRestaurantRepository> _mockRestaurant;
        private Mock<IGebruikerRepository> _mockGebruiker;
        private Mock<IReservatieRepository> _mockReservatie;

        private Microsoft.Extensions.Logging.ILoggerFactory _nullLogger; 

        private RestaurantService _restaurantService;
        private GebruikerService _gebruikerService;
        private ReservatieService _reservatieService;

        private RestaurantController _restaurantController;
        public RestaurantControllerTests()
        {
            _mockRestaurant = new Mock<IRestaurantRepository>();
            _mockGebruiker = new Mock<IGebruikerRepository>();
            _mockReservatie = new Mock<IReservatieRepository>();
           
            _restaurantService = new RestaurantService(_mockRestaurant.Object);
            _gebruikerService = new GebruikerService(_mockGebruiker.Object);
            _reservatieService = new ReservatieService(_mockReservatie.Object, _mockRestaurant.Object);

            _nullLogger = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory();
        }

        [Fact()]
        public void GetRestaurantTest_NotFound()
        {

            _restaurantController = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService, _nullLogger);

            _mockRestaurant.Setup(repo => repo.GeefRestaurant(2)).Throws(new GebruikerException("restaurant not found"));
            var res = _restaurantController.GetRestaurant(2);

            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        //[Fact()]
        //public void GetRestaurantTest_ValidID_Ok()
        //{
        //    //_restaurantController = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService, _nullLogger);

        //    //Restaurant restaurant = new Restaurant(1, "restaurant",
        //    //    new Locatie("4444", "Zelzate", "straat", 2), "open keueken", "/", null);

        //    //_mockRestaurant.Setup(repo => repo.HeefRestaurant(1)).Returns(true);
        //    //_mockRestaurant.Setup(repo => repo.GeefRestaurant(1)).Returns(restaurant);
        //    //var res = _restaurantController.GetRestaurant(1);

        //    //RestaurantRESToutputDTO output = ((RestaurantRESToutputDTO)((OkObjectResult)res.Result).Value);
        //    //string url = output.RestaurantURL;

        //    //Assert.IsType<OkObjectResult>(res.Result);
        //    //Assert.IsType<RestaurantRESToutputDTO>(output);
        //    ////Assert.Equal(MapFromDomain.MapFromGebruikerDomain(url, gebruiker) , output);
        //    //Assert.Equal(1, output.Id);
        //}


        // Posts 
        //[Fact()]
        //public void PostRestaurantTest_Invalid_BadRequest()
        //{
        //    _restaurantController = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService);

        //    RestaurantRESTinputDTO restaurantInput = new RestaurantRESTinputDTO("restaurant",
        //        new LocatieRESTinputDTO("4444", "Zelzate", "straat", 2), "open keueken", "/", null);

        //    Restaurant restaurant = MapToDomain.MapToRestaurantDomain(restaurantInput);

        //    _mockRestaurant.Setup(repo => repo.VoegRestaurantToe(restaurant)).Throws(new GebruikerException("restaurant not found"));
        //    var res = _restaurantController.PostRestaurant(restaurantInput);

        //    Assert.IsType<BadRequestObjectResult>(res.Result);
        //}

        [Theory()]
        [InlineData(1)]
        public void PostRestaurantTafelTest_Valid_OkResult(int restaurantId)
        {
            _restaurantController = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService, _nullLogger);

            List<TafelListRESTinputDTO> tafelListRESTinputDTOs = new List<TafelListRESTinputDTO>()
            {
                new TafelListRESTinputDTO(10)
            };

            _mockRestaurant.Setup(repo => repo.HeefRestaurant(restaurantId)).Returns(true);
            var res = _restaurantController.PostRestaurantTafel(restaurantId, tafelListRESTinputDTOs);

            Assert.IsType<OkResult>(res.Result);

        }

        [Fact()]
        public void DeleteRestaurantTest_Valid_NoContent()
        {
            _restaurantController = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService, _nullLogger);

            _mockRestaurant.Setup(repo => repo.HeefRestaurant(1)).Returns(true);
            var res = _restaurantController.DeleteRestaurant(1);

            Assert.IsType<NoContentResult>(res);
        }

        //    [Theory()]
        //    [InlineData(1, 1)]
        //    public void PostReservatieTest_Valid_CreatedAtAction(int restaurantId, int gebruikerId)
        //    {

        //        _restaurantController = new RestaurantController(_restaurantService, _reservatieService, _gebruikerService);

        //        ReservatieRESTinputDTO reservatieRESTinputDTO = new ReservatieRESTinputDTO(restaurantId, gebruikerId, 10, new DateTime(2023, 2, 3));
        //        Restaurant restaurant = new Restaurant(1, "Restu", new Locatie("2222", "Zelzate", "Langelede", 33), "open keuken", null);
        //        Gebruiker gebruiker = new Gebruiker(1, "Tuur", "tuur.vanholen@outlook.com", "0499109345",
        //            new Locatie("9185", "Wachtebeke", "Langelede", 42));

        //        _mockRestaurant.Setup(repo => repo.GeefRestaurant(restaurantId)).Returns(restaurant);
        //        _mockGebruiker.Setup(repo => repo.GeefGebruiker(gebruikerId)).Returns(gebruiker);

        //        var res = _restaurantController.PostReservatie(reservatieRESTinputDTO);

        //        Assert.IsType<CreatedAtActionResult>(res.Result); 
        //    }
        //}
    }
}