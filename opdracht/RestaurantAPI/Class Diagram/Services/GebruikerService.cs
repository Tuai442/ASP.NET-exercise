using System;
using RestaurantBL.Exceptions;
using RestaurantBL.Interfaces;
using RestaurantBL.Models;

namespace RestaurantBL.Services
{
    public class GebruikerService
    {
        private IGebruikerRepository _gebruikerRepo;

        public GebruikerService(IGebruikerRepository gebruikerRepo)
        {
            _gebruikerRepo = gebruikerRepo;
        }
        public Gebruiker UpdateGebruiker(Gebruiker gebruiker)
        {
            try
            {
                if (gebruiker == null) throw new GebruikerServiceException("UpdateGebruiker - null");
                if (!_gebruikerRepo.HeeftGebruiker(gebruiker.KlantNr)) throw new GebruikerServiceException("UpdateGebruiker - bestaat niet");
                Gebruiker gebruikerDB = _gebruikerRepo.GeefGebruiker(gebruiker.KlantNr);
                if (gebruiker == gebruikerDB) throw new GebruikerServiceException("UpdateGebruiker - geen update");
                _gebruikerRepo.UpdateGebruiker(gebruiker);
                return gebruiker;
            }
            catch (Exception ex)
            {
                throw new GebruikerServiceException("UpdateGemeente", ex);
            }
        }
        public Gebruiker VoegGebruikerToe(Gebruiker gebruiker)
        {
            
            _gebruikerRepo.VoegGebruikerToe(gebruiker);
            return gebruiker;
        }

        public Gebruiker GeefGebruiker(int id)
        {
            try
            {
                return _gebruikerRepo.GeefGebruiker(id);
            }
            catch(Exception ex)
            {
                throw new GebruikerServiceException("GeefGebruiker", ex);
            }
          
        }

        public bool BestaatGebruiker(int klantNr)
        {
            try
            {
                return _gebruikerRepo.HeeftGebruiker(klantNr);
            }
            catch (Exception ex)
            {
                throw new GebruikerServiceException("BestaatGebruiker", ex);
            }
        }

        public void VerwijderGebruiker(int klantNr)
        {
            try
            {
                if (!_gebruikerRepo.HeeftGebruiker(klantNr)) throw new GebruikerServiceException("Verwijdergebruiker - gebruiker bestaat niet");
                //if (_gebruikerRepo.HeeftStraten(gemeenteId)) throw new GebruikerServiceException("Verwijdergebruiker - straten niet leeg");
                Gebruiker gebruiker = _gebruikerRepo.GeefGebruiker(klantNr);
                _gebruikerRepo.VerwijderGebruiker(klantNr, gebruiker.Locatie.Id);
            }
            //catch (GemeenteServiceException ex) { throw new Exception("test",ex); }
            catch (Exception ex)
            {
                throw new GebruikerServiceException("Verwijdergebruiker", ex);
            }
        }

        public int GeefLocatieId(int id)
        {
            try
            {
                if (id == null) throw new GebruikerServiceException("GeefLocatie - null");
                if (!_gebruikerRepo.HeeftGebruiker(id)) throw new GebruikerServiceException("GeefLocatie - gebruiker bestaat niet");
                Locatie locatie = _gebruikerRepo.GeefLocatie(id);
                return locatie.Id;
            }
            catch (GebruikerServiceException) { throw; }
            catch (Exception ex)
            {

                throw new GebruikerServiceException("GeefLocatie", ex);
            }
        }
    }
}

