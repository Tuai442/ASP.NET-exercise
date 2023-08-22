using RestaurantBL.Models;
using System;

public class Reservatie
{

    private TimeSpan RESERVATIE_LENGTE = new TimeSpan(0, 1, 30, 0);
    private DateTime _datum;
    private int _aantalPlaatsen;

    public int ReservatieNr { get; set; }
    //public Restaurant Restaurant { get; set; }
    public Restaurant Restaurant { get; set; }
    public Gebruiker Gebruiker { get; set; }
    public int AantalPlaatsen { 
        get => _aantalPlaatsen; 
        set
        {
            CheckPlaatsen(value, Tafel);
            _aantalPlaatsen = value;
        } 
    }
    public DateTime Datum { 
        get => _datum;
        set
        {
            ControleerDatum(value);
            _datum = value;
            
        } 
    }

    private void ControleerDatum(DateTime datum)
    {
        if ((datum.Minute != 30 || datum.Minute != 00) && datum.Second != 00 && datum.Millisecond != 00) throw new ReservatieException("Datum moet op een rond uur / half uur beginnen");
    }

    public Tafel Tafel { get; set; }

    public Reservatie(Gebruiker gebruiker, Restaurant restaurant, int aantalPlaatsen, DateTime datum, Tafel tafel)
    {
        
        Restaurant = restaurant;
        Gebruiker = gebruiker;
    
        Datum = datum;
        Tafel = tafel;
        AantalPlaatsen = aantalPlaatsen;
    }

    public Reservatie(int reservatieNr, Gebruiker gebruiker, Restaurant restaurant, int aantalPlaatsen, DateTime datum, Tafel tafel)
    {
        ReservatieNr = reservatieNr;
        Restaurant = restaurant;
        Gebruiker = gebruiker;
        
        Datum = datum;
        Tafel = tafel;
        AantalPlaatsen = aantalPlaatsen;
        
        //TafelNr = tafelNr;
    }

    private void CheckPlaatsen(int aantalPlaatsen, Tafel tafel)
    {
        if (tafel.Plaatsen < aantalPlaatsen)
        {
            throw new ReservatieException("De tafel heeft te weinig plaatsen om te reserveren");
        }
        if(aantalPlaatsen <= 0)
        {
            throw new ReservatieException("Het aantal plaatsen moet groten zijn dan 0");
        }
    }

    public override bool Equals(object? obj)
    {
        Reservatie t2 = obj as Reservatie;
        if (t2 == null)
            return false;
        else
            return this.ReservatieNr == t2.ReservatieNr;
    }

    public DateTime StartTijd()
    {
        return Datum.Add(-RESERVATIE_LENGTE);
    }
    public DateTime EindDatum()
    {
        return Datum.Add(RESERVATIE_LENGTE);
    }
    public override int GetHashCode()
    {
        return this.ReservatieNr;
    }
}
