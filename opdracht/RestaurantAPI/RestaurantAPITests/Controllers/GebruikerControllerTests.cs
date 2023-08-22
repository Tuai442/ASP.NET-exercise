using Xunit;
using RestaurantAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantAPI.Model.Input;
using RestaurantBL.Interfaces;
using Xunit.Sdk;
using RestaurantBL.Services;
using Microsoft.AspNetCore.Builder;
using System.Net.Mail;
using AdresbeheerREST.Mappers;

namespace RestaurantAPI.Controllers.Tests
{
    public class GebruikerControllerTests
    {
        private GebruikerService gebruikerService;
        private GebruikerController _controller;
        private List<GebruikerRESTinputDTO> gebruikers = new List<GebruikerRESTinputDTO>
        {
           // new GebruikerRESTinputDTO("Tuur", "Van Holen", "000", "Gent", 0)
        };

        public GebruikerControllerTests()
        {
            _controller = new GebruikerController(gebruikerService);
        }

        [Theory]
        [InlineData("", "Van Holen", "000", "Gent", 0)]

        public void PostGebruikerTest_InValidData(string naam, string email, string telefoonNr, string locatie, int klantNr)
        {
            
            
        }
    }
}