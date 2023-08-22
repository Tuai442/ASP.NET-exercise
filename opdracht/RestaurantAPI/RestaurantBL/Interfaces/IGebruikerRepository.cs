using RestaurantBL.Models;
using System;


namespace RestaurantBL.Interfaces
{
    public interface IGebruikerRepository
    {
        void UpdateGebruiker(Gebruiker gebruiker);
        void VoegGebruikerToe(Gebruiker gebruiker);
        Gebruiker GeefGebruiker(int id);
        bool HeeftGebruiker(int klantNr);
        void VerwijderGebruiker(int klantNr, int locatieId);
        Locatie GeefLocatie(int id);
    }
}

