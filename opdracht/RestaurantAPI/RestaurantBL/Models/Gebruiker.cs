using RestaurantBL.Interfaces;
using RestaurantBL.Models;
using System;
using System.Text.RegularExpressions;

public class Gebruiker 
{
	private string _naam;
	private string _email;
	private string _telefoonNr;
	private int klantNr;
	
	
	public string Naam { get => _naam;
        set
        {
            if (string.IsNullOrWhiteSpace(value)) throw new GebruikerException("naam not valid"); else _naam = value;
        }
    }
	public string Email { get => _email; 
        set
        {
            Controleer.Email(value);
            _email = value;
        } }
	public string TelefoonNr { get => _telefoonNr; 
        set
        {
            Controleer.Telefoon(value);
            _telefoonNr = value;
        }
    }
	public Locatie Locatie { get; set; }
    
	public int KlantNr { get; set; }
    public Gebruiker(string naam, 
		string email, string telefoonNr, Locatie locatie)
	{
		Naam = naam;
		Email = email;
		TelefoonNr = telefoonNr;
		Locatie = locatie;
		//KlantNr = klantNr;

        //ZetNIScode(nIScode);
    }

    public Gebruiker(int klantNr, string naam, string email, string telefoonNr, Locatie locatie)
    {
        Naam = naam;
        Email = email;
        TelefoonNr = telefoonNr;
        Locatie = locatie;
        KlantNr = klantNr;

    }

    public void ZetId(int id)
    {
        if(id <= 0)
        {
            throw new GebruikerException("KlantNr moet hoger zijn dan 0");
        }
        KlantNr= id;
    }

    public int GeefId()
    {
        return KlantNr;
    }

   
}
