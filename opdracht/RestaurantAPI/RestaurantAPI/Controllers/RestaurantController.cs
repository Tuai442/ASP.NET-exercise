using AdresbeheerREST.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Mappers;
using RestaurantAPI.Model.Input;
using RestaurantAPI.Model.Input.Restaurant;
using RestaurantAPI.Model.Input.Restaurant.Reservatie;
using RestaurantAPI.Model.Input.Restaurant.Tafel;
using RestaurantAPI.Model.Output;
using RestaurantBL;
using RestaurantBL.Models;
using RestaurantBL.Services;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private ReservatieService _reservatieService;
        private RestaurantService _restaurantService;
        private GebruikerService _gebruikerService;
        private readonly ILogger _logger;

        private string hostURL = "http://localhost:5013/api/restaurantbeheer";

        public RestaurantController(RestaurantService restaurantService, ReservatieService reservatieService,
            GebruikerService gebruikerService, ILoggerFactory loggerFactory)
        {
            _restaurantService = restaurantService;
            _reservatieService = reservatieService;
            _gebruikerService = gebruikerService;

            _logger = loggerFactory.AddFile("RestaurantControllerLogs.txt").CreateLogger("Restaurant");
        }

        // Post
        [HttpPost]
        [Route("VoegRestaurantToe")]
        public ActionResult<RestaurantRESTinputDTO> PostRestaurant([FromBody] RestaurantRESTinputDTO restaurantDTO)
        {
            try
            {
                _logger.LogInformation("PostRestaurant called");
                Restaurant restaurant = _restaurantService.VoegRestaurantToe(MapToDomain.MapToRestaurantDomain(restaurantDTO));
                return CreatedAtAction(nameof(GetRestaurant), new { id = restaurant.Id }, restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError($"PostRestaurant error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("VoegTafelToe")]
        public ActionResult<List<TafelListRESToutputDTO>> PostRestaurantTafel(int restaurantId, [FromBody] List<TafelListRESTinputDTO> restDTO)
        {
            try
            {
                _logger.LogInformation("PostRestaurantTafel called");

                if (_restaurantService.BestaatRestaurant(restaurantId))
                {
                    List < Tafel > tafels = _restaurantService.VoegTafelsToeRestaurant(restaurantId, MapToListDomain.MapToTafelDomain(restDTO));
                    List<TafelListRESToutputDTO> dto = 
                        tafels.Select(x => MapToList.MapToListTafels(hostURL, x)).ToList();
                    return Ok();
                }
                else
                {
                    return NotFound("tafel niet gevonden");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"PostRestaurantTafel error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // Get
        [HttpGet("{id}")]
        public ActionResult<RestaurantRESToutputDTO> GetRestaurant(int id)
        {
            try
            {
                _logger.LogInformation("GetRestaurant called");
                Restaurant restaurant = _restaurantService.GeefRestaurant(id);
                return Ok(MapFromDomain.MapFromRestaurantDomain(hostURL, restaurant));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetRestaurant error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetReservatiesOpDatums/{beginDatum}/{eindDatum}")]
        public ActionResult<List<RestaurantListRESToutputDTO>> GetReservatiesOpDatums(DateTime beginDatum, DateTime eindDatum)
        {
            try
            {
                _logger.LogInformation("GetReservatieOpDatum called");

                List<ReservatieRESToutputDTO> restaurants = _reservatieService.GeefReservatieOpDatum(beginDatum, eindDatum).
                    Select(x => MapFromDomain.MapFromReservatieDomain(hostURL, x)).ToList();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReservatieOpDatum error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        // Put
        [HttpPut("UpdateRestaurant/{id}")]
        public IActionResult PutRestaurant(int id, [FromBody] RestaurantUpdateRESTinputDTO restDTO)
        {
            try
            {
                _logger.LogInformation("PutRestaurant called");

                if (_restaurantService.BestaatRestaurant(id))
                {
                    Restaurant restaurant = MapToDomain.MapToRestaurantUpdateDomain(id, restDTO);
                    Restaurant restaurantUpdated = _restaurantService.UpdateRestaurant(restaurant);
                    
                    return CreatedAtAction(nameof(GetRestaurant), new { id = restaurantUpdated.Id },
                        MapFromDomain.MapFromRestaurantDomain(hostURL, restaurantUpdated));
                }
                else
                {
                    _logger.LogWarning($"restaurant niet gevonden");
                    return NotFound("restaurant niet gevonden");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"PutRestaurant error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

      
        // Delete
        [HttpDelete("DeleteRestaurant/{id}")]
        public IActionResult DeleteRestaurant(int id)
        {
            try
            {
                _logger.LogInformation("DeleteRestaurant called");
                _restaurantService.VerwijderRestaurant(id);
                return NoContent();
            }
            
            catch (Exception ex)
            {
                _logger.LogError($"DeleteRestaurant error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteTafel/{tafelId}")]
        public IActionResult DeleteTafel(int tafelId)
        {
            try
            {
                _logger.LogInformation("DeleteTafel called");
                _restaurantService.VerwijderTafel(tafelId);
                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError($"DeleteTafel error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }


    }


}
