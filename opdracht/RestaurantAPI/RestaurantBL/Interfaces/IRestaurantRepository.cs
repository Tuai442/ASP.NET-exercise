using RestaurantBL.Models;
using System;


namespace RestaurantBL.Interfaces
{
    public interface IRestaurantRepository
    {
        List<Restaurant> GeefRestaurantOpLocatieKeuken(Locatie locatie, string keuken);
        void VoegRestaurantToe(Restaurant restaurant);
        void VerwijderRestaurant(Restaurant restaurant);
        void UpdateRestaurant(Restaurant restaurant);
        Restaurant GeefRestaurant(int id);
        bool HeefRestaurant(int id);
       // List<Restaurant> GeefRestaurantOpDatum(DateTime datum);
        void VoegTafelsToe(int id, List<Tafel> tafels);
        Tafel GeefTafel(int id);
        Locatie GeefLocatie(int restaurantId);

        Locatie GeefLocatieOpPostCode(string gemeente);
        bool HeeftTafel(int id);
        void VerwijderTafel(int tafelId);
        Restaurant GeefRestaurantOpTafelId(int tafelId);
    }

}

