using AdresbeheerREST.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Mappers;
using RestaurantAPI.Model.Input;
using RestaurantAPI.Model.Input.Restaurant.Reservatie;
using RestaurantAPI.Model.Output;
using RestaurantBL.Exceptions;
using RestaurantBL.Services;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {
        private GebruikerService _gebruikerService;
        private RestaurantService _restaurantService;
        private ReservatieService _reservatieService;
        private readonly ILogger _logger;

        private string hostURL = "http://localhost:5013/api/restaurantbeheer";

        public GebruikerController(GebruikerService gebruikerService, RestaurantService restaurantService,
            ReservatieService reservatieService, ILoggerFactory loggerFactory)
        {
            _gebruikerService = gebruikerService;
            _restaurantService = restaurantService;
            _reservatieService = reservatieService;
            _logger = loggerFactory.AddFile("GebruikerControllerLogs.txt").CreateLogger("Gebruiker");
        }

        // Post
        [HttpPost("PostGebruiker")]
        public ActionResult<GebruikerRESTinputDTO> PostGebruiker([FromBody] GebruikerRESTinputDTO gebruikerDTO)
        {
            try
            {
                _logger.LogInformation("PostGebruiker called");
                Gebruiker gebruiker = _gebruikerService.VoegGebruikerToe(MapToDomain.MapToGebruikerDomain(gebruikerDTO));
                return CreatedAtAction(nameof(GetGebruiker), new { klantNr = gebruiker.KlantNr },
                    MapFromDomain.MapFromGebruikerDomain(hostURL, gebruiker));
            }
            catch (Exception ex)
            {
                _logger.LogError($"PostGebruiker error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PostReservatie")]
        public ActionResult<ReservatieRESTinputDTO> PostReservatie([FromBody] ReservatieRESTinputDTO reservatieDTO)
        {
            try
            {
                _logger.LogInformation("PostReservatie called");

                Restaurant restaurant = _restaurantService.GeefRestaurant(reservatieDTO.RestaurantId);
                if (restaurant == null) return NotFound("Restaurant niet gevonden");
                Gebruiker gebruiker = _gebruikerService.GeefGebruiker(reservatieDTO.KlantNr);
                if (gebruiker == null) return NotFound("Restaurant niet gevonden");

                Reservatie reservatie = _reservatieService.VoegReservatieToe(restaurant, gebruiker,
                    reservatieDTO.AantalPlaatsen, reservatieDTO.Datum);
                return CreatedAtAction(nameof(GetReservatie), new { reservatieNr = reservatie.ReservatieNr },
                    MapFromDomain.MapFromReservatieDomain(hostURL, reservatie));
            }

            catch (Exception ex)
            {
                _logger.LogError($"PostReservatie error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        // Get
        [HttpGet("GetGebruiker/{klantNr}")]
        public ActionResult<GebruikerRESToutputDTO> GetGebruiker(int klantNr)
        {
            try
            {
                _logger.LogInformation("GetGebruiker called");
                Gebruiker gebruiker = _gebruikerService.GeefGebruiker(klantNr);
                if (gebruiker == null) throw new Exception("Gebruiker niet gevonden");
                return Ok(MapFromDomain.MapFromGebruikerDomain(hostURL, gebruiker));
            }

            catch (Exception ex)
            {
                _logger.LogError($"GetGebruiker error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetTafel/zoekdatum/{id}/{datum}/{aantalPlaatsen}")]
        public ActionResult<RestaurantRESToutputDTO> GetTafel(int id, DateTime datum, int aantalPlaatsen)
        {
            try
            {
                _logger.LogInformation("GetTafel called");
                List<TafelListRESToutputDTO> restaurants = _restaurantService.GeefVrijePlaatsenRestaurantOpIdDatum(id, datum, aantalPlaatsen).
                    Select(x => MapToList.MapToListTafels(hostURL, x)).ToList();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetTafel error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        //public ActionResult<List<RestaurantListRESToutputDTO>> GetRestaurantOp(string locatie, string keuken)
        //{
        //    try
        //    {
        //        _logger.LogInformation("GetRestaurantOp called");

        //        List<RestaurantListRESToutputDTO> restaurants = _restaurantService.GeefRestaurantOpLocatieKeuken(locatie, keuken).
        //            Select(x => MapToList.MapToListRestaurant(hostURL, x)).ToList();
        //        return Ok(restaurants);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"GetRestaurantOp error: {ex.Message}");
        //        return NotFound(ex.Message);
        //    }
        //}

        [HttpGet("GetReservatie/{reservatieNr}")]
        public ActionResult<ReservatieRESToutputDTO> GetReservatie(int reservatieNr)
        {
            try
            {
                _logger.LogInformation("GetReservatie called");
                Reservatie reservatie = _reservatieService.GeefReservatie(reservatieNr);
                if (reservatie == null)
                {
                    _logger.LogWarning("GetReservatie: reservatie niet gevonden");
                    return NotFound("Reservatie niet gevonden");
                }
                return Ok(MapFromDomain.MapFromReservatieDomain(hostURL, reservatie));
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetReservatie error: {ex.Message}");
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
        [HttpPut("UpdateGebruiker/{klantNr}")]
        public IActionResult PutGebruiker(int klantNr, [FromBody] GebruikerRESTinputDTO restDTO)
        {
            try
            {
                _logger.LogInformation("PutGebruiker called");
                if (_gebruikerService.BestaatGebruiker(klantNr))
                {
                    Gebruiker gebruiker = MapToDomain.MapToGebruikerDomain(klantNr, restDTO, _gebruikerService);
                    Gebruiker gebruikerUpdate = _gebruikerService.UpdateGebruiker(gebruiker);
                    return CreatedAtAction(nameof(GetGebruiker), new { klantNr = gebruiker.KlantNr }, MapFromDomain.MapFromGebruikerDomain(hostURL, gebruiker));
                }
                else
                {
                    _logger.LogWarning($"Gebruiker niet gevonden");
                    return NotFound("Gebruiker niet gevonden");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"PutGebruiker error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateReservatie/{reservatieNr}")]
        public IActionResult PutReservatie(int reservatieNr, [FromBody] ReservatieUpdateRESTinputDTO restDTO)
        {
            try
            {
                _logger.LogInformation("PutReservatie called");
                if (_reservatieService.BestaatReservatie(reservatieNr))
                {
                    Reservatie reservatie = _reservatieService.UpdateReservatie(reservatieNr, restDTO.AantalPlaatsen, restDTO.Datum);

                    return CreatedAtAction(nameof(GetReservatie), new { reservatieNr = reservatieNr },
                        MapFromDomain.MapFromReservatieDomain(hostURL, reservatie));
                }
                else
                {
                    _logger.LogWarning($"reservatie niet gevonden");
                    return NotFound("reservatie niet gevonden");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"PutReservatie error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


        // Delete
        [HttpDelete("DeleteGebruiker/{klantNr}")]
        public IActionResult DeleteGebruiker(int klantNr)
        {
            try
            {
                _logger.LogInformation("DeleteGemeente called");
                _gebruikerService.VerwijderGebruiker(klantNr);
                return NoContent();
            }
            catch (GebruikerServiceException ex)
            {
                _logger.LogError($"DeleteGemeente error: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteGemeente error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("DeleteReservatie/{id}")]
        public IActionResult DeleteReservatie(int id)
        {
            try
            {
                _logger.LogInformation("DeleteReservatie called");
                _reservatieService.VerwijderReservatie(id);
                return NoContent();
            }

            catch (Exception ex)
            {
                _logger.LogError($"DeleteReservatie error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
    }
}
