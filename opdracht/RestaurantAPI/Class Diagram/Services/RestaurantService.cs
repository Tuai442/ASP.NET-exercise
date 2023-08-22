using System;
using System.Text.RegularExpressions;
using RestaurantBL.Exceptions;
using RestaurantBL.Interfaces;
using RestaurantBL.Models;

namespace RestaurantBL.Services
{
    public class RestaurantService
    {
        private IRestaurantRepository _restaurantRepo;
        public RestaurantService(IRestaurantRepository restaurantRepo)
        {
            _restaurantRepo = restaurantRepo;

        }
        public List<Restaurant> GeefRestaurantOpLocatieKeuken(string locatie, string keuken)
        {
            try
            {
                Controleer.PostCode(locatie);
                Locatie loc = _restaurantRepo.GeefLocatieOpPostCode(locatie);
                if (loc == null) throw new RestaurantServiceException("Er zijn geen restaurants gevonden op locatie");
                
                return _restaurantRepo.GeefRestaurantOpLocatieKeuken(loc, keuken);
            }
            catch(LocatieExcepiton) { throw; }
            catch(RestaurantServiceException) { throw; }
            catch(Exception ex)
            {
                throw new RestaurantServiceException("GeefRestaurantOpLocatieKeuken", ex);
            }
            
        }
     
        public Restaurant VoegRestaurantToe(Restaurant restaurant)
        {
            _restaurantRepo.VoegRestaurantToe(restaurant);
            return restaurant;
        }
        public void VerwijderRestaurant(int id)
        {
            
            try
            {
                if (!_restaurantRepo.HeefRestaurant(id)) throw new RestaurantServiceException("Verwijderrestaurant - restaurant bestaat niet");
                Restaurant restaurant = _restaurantRepo.GeefRestaurant(id);
                _restaurantRepo.VerwijderRestaurant(restaurant);
            }
            catch(RestaurantServiceException) { throw; }
            catch (Exception ex)
            {
                throw new RestaurantServiceException("Verwijderrestaurant", ex);
            }
            
        }
        public Restaurant UpdateRestaurant(Restaurant restaurant)
        {
            try
            {
                if (restaurant == null) throw new RestaurantServiceException("UpdateRestaurant - null");
                if (!_restaurantRepo.HeefRestaurant(restaurant.Id)) throw new RestaurantServiceException("UpdateRestaurant - bestaat niet");
                Restaurant restaurantDB = _restaurantRepo.GeefRestaurant(restaurant.Id);
                if (restaurant == restaurantDB) throw new RestaurantServiceException("UpdateRestaurant - geen update");
                int locatieId = restaurantDB.Locatie.Id;
                restaurant.Locatie.ZetId(locatieId);

                foreach (Tafel tafel in restaurant.Tafels)
                {
                    restaurantDB.TafelUpdateCheck(tafel);
                }
                
                _restaurantRepo.UpdateRestaurant(restaurant);
                return restaurant;
            }
            catch (RestaurantServiceException) { throw; }
            catch (RestaurantException) { throw; }
            catch (Exception ex)
            {
                
                throw new RestaurantServiceException("UpdateRestaurant", ex);
            }
        }

        public Restaurant GeefRestaurant(int id)
        {
            try
            {
                if (id == null) throw new RestaurantServiceException("GeefRestaurant - null");
                if (!_restaurantRepo.HeefRestaurant(id)) throw new RestaurantServiceException("GeefRestaurant - bestaat niet");
                return _restaurantRepo.GeefRestaurant(id);
            }
            catch (RestaurantServiceException) { throw; }
            catch (Exception ex)
            {

                throw new RestaurantServiceException("UpdateRestaurant", ex);
            }
            
        }

        public bool BestaatRestaurant(int id)
        {
            try
            {
                return _restaurantRepo.HeefRestaurant(id);
            }
            catch (Exception ex)
            {
                throw new RestaurantServiceException("BestaatRestaurant", ex);
            }
        }

        public List<Tafel> GeefVrijePlaatsenRestaurantOpIdDatum(int id, DateTime datum, int aantalPlaatsen)
        {
            Restaurant restaurant = _restaurantRepo.GeefRestaurant(id);
            return restaurant.GeefVrijePlaatsen(0, datum, aantalPlaatsen);
            
        }

        public List<Tafel> VoegTafelsToeRestaurant(int restaurantId, List<Tafel> tafels)
        {
            try
            {
                if (!_restaurantRepo.HeefRestaurant(restaurantId)) throw new RestaurantServiceException("Restaurant bestaat niet");
                
                _restaurantRepo.VoegTafelsToe(restaurantId, tafels);
                return tafels;
            }
            catch(RestaurantServiceException) { throw; }
            catch (Exception ex)
            {
                throw new RestaurantServiceException("RestaurantServiceExcepion", ex);
            }
           

        }

        public Tafel GeefTafel(int id)
        {
            return _restaurantRepo.GeefTafel(id);
        }

        public int GeefLocatieId(int restaurantId)
        {
            try
            {
                if (restaurantId == null) throw new RestaurantServiceException("GeefLocatie - null");
                if (!_restaurantRepo.HeefRestaurant(restaurantId)) throw new RestaurantServiceException("GeefLocatie - restaurant bestaat niet");
                Locatie locatie = _restaurantRepo.GeefLocatie(restaurantId);
                return locatie.Id;
            }
            catch (RestaurantServiceException) { throw; }
            catch (Exception ex)
            {

                throw new RestaurantServiceException("UpdateRestaurant", ex);
            }
        }

        public void VerwijderTafel(int tafelId)
        {

            try
            {
                Restaurant restaurant = _restaurantRepo.GeefRestaurantOpTafelId(tafelId);
                if (restaurant == null) throw new RestaurantServiceException("VerwijderTafel - restaurant bestaat niet");
                
                restaurant.VerwijderTafel(tafelId);
                _restaurantRepo.VerwijderTafel(tafelId);


            }
            catch (RestaurantException) { throw; }
            catch (RestaurantServiceException) { throw; }
            catch (Exception ex)
            {
                throw new RestaurantServiceException("Verwijderrestaurant", ex);
            }
        }
    }
}

