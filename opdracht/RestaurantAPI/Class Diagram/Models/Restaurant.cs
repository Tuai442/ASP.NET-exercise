using RestaurantBL.Interfaces;
using RestaurantBL.Models;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

public class Restaurant
{
    private List<Reservatie> _reservaties = new List<Reservatie>();
    private string _naam;
    private string _email;
    private string _telefoon;

    public int Id { get; set; }
    public string Naam
    {
        get => _naam;
        set
        {
            if (string.IsNullOrEmpty(value)) throw new RestaurantException("Naam mag niet leeg zijn");
            _naam = value;
        }
    }

    public Locatie Locatie { get; set; }
    public string Keuken { get; set; }
    public string ContactGegevens { get; set; }

    public string Email
    {
        get => _email;
        set
        {
            Controleer.Email(value);
            _email = value;
        }
    }
    public string Telefoon { get => _telefoon;
        set 
        {
            Controleer.Telefoon(value);
            _telefoon = value;
        }
    }
    public List<Reservatie> Reservaties { get => _reservaties; set => _reservaties = value; }
    public List<Tafel> Tafels { get; set; }

    public Restaurant(string naam, Locatie locatie, string keuken, string telefoon, string email, List<Tafel> tafels = null)
    {
        Naam = naam;
        Locatie = locatie;
        Keuken = keuken;

        Telefoon = telefoon;
        Email = email;
        Tafels = tafels;
    }
    public Restaurant(int id, string naam, Locatie locatie, string keuken, string telefoon, string email, List<Tafel> tafels = null)
    {
        Id = id;
        Naam = naam;
        Locatie = locatie;
        Keuken = keuken;

        Telefoon = telefoon;
        Email = email;
        Tafels = tafels;

    }

    public List<Tafel> GeefVrijePlaatsen(int reservatieNr, DateTime datum, int minAantalPlaatsen)
    {
        List<Tafel> result = Tafels.Where(x => x.Plaatsen >= minAantalPlaatsen).Select(x => x).ToList();
        foreach (Reservatie res in Reservaties)
        {
            if(res.ReservatieNr != reservatieNr)
            {
                if (res.StartTijd() < datum && res.EindDatum() > datum)
                {
                    result.Remove(res.Tafel);
                }
            }
            
        }
        return result;
    }

    public void VoegReservatieToe(Reservatie reservatie)
    {
        ControleerReservatie(reservatie);
        _reservaties.Add(reservatie);
    }

    public void ControleerReservatie(Reservatie reservatie)
    {
        if (!Tafels.Contains(reservatie.Tafel)) throw new ReservatieException("De tafel bestaat niet");

        foreach (Reservatie res in Reservaties)
        {
            if (res.Tafel.Equals(reservatie.Tafel) && !res.Equals(reservatie))
            {
                // De startTijd is de tijdstip - 30min voor overlaps te voorkomen
                if (res.StartTijd() < reservatie.Datum && res.EindDatum() > reservatie.Datum)
                {
                    throw new ReservatieException("De Tafel is bezet");
                }
            }

        }
    }

    public void ZetId(int id)
    {
        if (id <= 0)
        {
            throw new RestaurantException("Id moet groter dan 0 zijn.");
        }
        Id = id;
    }

    public int GeefId()
    {
        return Id;
    }

    public void TafelUpdateCheck(Tafel tafel)
    {
        if (!Tafels.Contains(tafel)) throw new RestaurantException("De tafel bestaat niet in dit restaurant");
        foreach (Reservatie reservatie in Reservaties)
        {
            if (reservatie.Tafel.Equals(tafel) && reservatie.Datum > DateTime.Now)
            {
                if (reservatie.AantalPlaatsen > tafel.Plaatsen)
                {
                    throw new RestaurantException($"Je kan de tafel niet updaten - hier is een reservatie voor {reservatie.AantalPlaatsen} plaatsen gemaakt.");
                }
            }
        }
    }

    public Tafel GeefVrijeTafel(int reservatieNr, DateTime datum, int aantalPlaatsen)
    {
        List<Tafel> vrijePlaatsen = GeefVrijePlaatsen(reservatieNr, datum, aantalPlaatsen);

        Tafel bestPassendeTafel = null;

        foreach (Tafel tafel in vrijePlaatsen)
        {
            if (tafel.Plaatsen == aantalPlaatsen)
            {
                return tafel;
            }
            else if (bestPassendeTafel != null)
            {
                int a = aantalPlaatsen - tafel.Plaatsen;
                int b = aantalPlaatsen - bestPassendeTafel.Plaatsen;
                if (a > b)
                {
                    bestPassendeTafel = tafel;
                }
            }
            else
            {
                if (tafel.Plaatsen > aantalPlaatsen)
                {
                    bestPassendeTafel = tafel;
                }

            }

        }
        return bestPassendeTafel;
    }

    public void VerwijderTafel(int tafelNr)
    {
       Tafel tafelVerwijder = Tafels.Where(x => x.Id == tafelNr).FirstOrDefault();
       foreach(Reservatie reservatie in Reservaties)
        {
            if (reservatie.Tafel.Equals(tafelVerwijder) && reservatie.Datum > DateTime.Now)
            {
                throw new RestaurantException($"Je kan de tafel niet verwijderen zolang hier een reservatie mee verbonden is.");
            }
        }
    }
}
