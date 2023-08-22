using Xunit;
using RestaurantAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantBL.Interfaces;
using Xunit.Sdk;
using RestaurantBL.Services;
using RestaurantDL.Repositories;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Model.Input;
using Moq;
using RestaurantBL.Models;
using System.Diagnostics.Metrics;
using RestaurantAPI.Model.Output;
using AdresbeheerREST.Mappers;

namespace RestaurantAPI.Controllers.Tests
{
    public class GebruikerControllerTests
    {
        private Mock<IRestaurantRepository> _mockRestaurant;
        private Mock<IGebruikerRepository> _mockGebruiker;
        private Mock<IReservatieRepository> _mockReservatie;

        private Microsoft.Extensions.Logging.ILoggerFactory _nullLogger;


        private RestaurantService _restaurantService;
        private GebruikerService _gebruikerService;
        private ReservatieService _reservatieService;

        private GebruikerController _gebruikerController;
        

        public GebruikerControllerTests()
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
        public void GetGebruikerTest_NotFound()
        {
            _gebruikerController = new GebruikerController(_gebruikerService, _restaurantService, _reservatieService, _nullLogger);

            _mockGebruiker.Setup(repo => repo.GeefGebruiker(2)).Throws(new GebruikerException("gebruiker not found"));
            var res = _gebruikerController.GetGebruiker(2);

            Assert.IsType<NotFoundObjectResult>(res.Result);
        }

        [Fact()]
        public void GetGebruikerTest_ValidID_Ok()
        {
            _gebruikerController = new GebruikerController(_gebruikerService, _restaurantService, _reservatieService, _nullLogger);
            Gebruiker gebruiker = new Gebruiker(1, "Tuur", "tuur.vanholen@outlook.com", "0499109345", 
                new Locatie("9185", "Wachtebeke", "Langelede", 42));

            _mockGebruiker.Setup(repo => repo.GeefGebruiker(1)).Returns(gebruiker);
            var res = _gebruikerController.GetGebruiker(1);

            GebruikerRESToutputDTO output = ((GebruikerRESToutputDTO)((OkObjectResult)res.Result).Value);
            string url = output.GebruikerURL;

            Assert.IsType<OkObjectResult>(res.Result);
            Assert.IsType<GebruikerRESToutputDTO>(output);
            Assert.Equal(1, output.KlantNr);
        }

        //[Fact]
        //public void PostGebruikerTest_Invalid_BadRequest()
        //{
        //    _gebruikerController = new GebruikerController(_gebruikerService, _restaurantService, _reservatieService, null);

        //    GebruikerRESTinputDTO gebruikerInput = new GebruikerRESTinputDTO("Tuur", "tuur.vanholen@outlook.com", "0499109345",
        //        new LocatieRESTinputDTO("9185", "Wachtebeke", "Langelede", 42));
        //    Gebruiker gebruiker = MapToDomain.MapToGebruikerDomain(gebruikerInput);

        //    _mockGebruiker.Setup(repo => repo.VoegGebruikerToe(gebruiker)).Throws(new GebruikerException("gebruiker not found"));
        //    var res = _gebruikerController.PostGebruiker(gebruikerInput);

        //    Assert.IsType<BadRequestObjectResult>(res.Result);
        //}


        [Fact]
        public void PostGebruikerTest_Valid_CreatedAtAction()
        {
            _gebruikerController = new GebruikerController(_gebruikerService, _restaurantService, _reservatieService, _nullLogger);

            GebruikerRESTinputDTO gebruikerInput = new GebruikerRESTinputDTO("Tuur", "tuur.vanholen@outlook.com", "0499109345",
                new LocatieRESTinputDTO("9185", "Wachtebeke", "Langelede", 42));

            var res = _gebruikerController.PostGebruiker(gebruikerInput);

            Assert.IsType<CreatedAtActionResult>(res.Result);
        }

        [Fact]
        public void PutGebruikerTest_Valid_CreatedAtAction()
        {
            _gebruikerController = new GebruikerController(_gebruikerService, _restaurantService, _reservatieService, _nullLogger);

            GebruikerRESTinputDTO gebruikerInput = new GebruikerRESTinputDTO("Tuur", "tuur.vanholen@outlook.com", "0499109345",
                new LocatieRESTinputDTO("9185", "Wachtebeke", "Langelede", 42));

            _mockGebruiker.Setup(repo => repo.HeeftGebruiker(1)).Returns(false);
            var res = _gebruikerController.PostGebruiker(gebruikerInput);

            Assert.IsType<CreatedAtActionResult>(res.Result);
        }
    }
}