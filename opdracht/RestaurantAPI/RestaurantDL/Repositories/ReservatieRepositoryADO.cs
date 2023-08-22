using RestaurantBL.Interfaces;
using RestaurantBL.Models;
using RestaurantDL.Exceptions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantDL.Repositories
{

    public class ReservatieRepositoryADO : IReservatieRepository
    {
        private string _connectionString;

        public ReservatieRepositoryADO(string connection)
        {
            _connectionString = connection;

        }

        public void AnnuleerReservatie(ref int id)
        {
            throw new System.NotImplementedException("Not implemented");
        }

        public void VoegReservatieToe(Reservatie reservatie)
        {
            string sql = "insert into Reservatie(aantal_plaatsen, datum, tafel_id, contact_persoon_id, restaurant_id) " +
                "output INSERTED.reservatie_nr " +
                "VALUES(@aantal_plaatsen, @datum, @tafel_id, @contact_persoon_id, @restaurant_id)";

            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@aantal_plaatsen", reservatie.AantalPlaatsen);
                    cmd.Parameters.AddWithValue("@datum", reservatie.Datum);
                    cmd.Parameters.AddWithValue("@tafel_id", reservatie.Tafel.Id);
                    cmd.Parameters.AddWithValue("@contact_persoon_id", reservatie.Gebruiker.KlantNr);
                    cmd.Parameters.AddWithValue("@restaurant_id", reservatie.Restaurant.Id);

                    reservatie.ReservatieNr = (int)cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new ReservatieRepositoryException("VoegReservatieToe", ex);
                }
                finally { con.Close(); }
            }
        }

        public Reservatie GeefReservatie(int reservatieNr)
        {
            string query = "SELECT *, r.id as restaurant_id, r.naam as resto_naam, t.id as tafel_id, " +
                "lg.postcode as lg_postcode, lg.gemeente as lg_gemeente, lg.straat as lg_straat, lg.huisNr as lg_huisnr , " +
                "lr.postcode as lr_postcode, lr.gemeente as lr_gemeente, lr.straat as lr_straat, lr.huisNr as lr_huisnr, " +
                "g.email as g_email, r.email as r_email " +
                "FROM Reservatie res " +
                "join Gebruiker g on g.klant_nr=res.contact_persoon_id " +
                "join Restaurant r on r.id=res.restaurant_id  " +
                "join Tafel t on t.id = res.tafel_id " +
                "left join Locatie lg on g.locatie_id = lg.id " +
                "left join locatie lr on r.locatie_id = lr.id " +
                "where res.reservatie_nr=@reservatieNr ";
            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@reservatieNr", reservatieNr);
                    IDataReader dataReader = cmd.ExecuteReader();
                    Reservatie reservatie = null;
                    if (dataReader.Read())
                    {
                        int resNr = (int)dataReader["reservatie_nr"];
                        int aantalPlaatsen = (int)dataReader["aantal_plaatsen"];
                        int tafelNr = (int)dataReader["tafel_id"];
                        DateTime datum = (DateTime)dataReader["datum"];

                        // Locatie - Gebruiker
                        string postCodeG = (string)dataReader["lg_postcode"];
                        string gemeenteG = (string)dataReader["lg_gemeente"];
                        string straatG = (string)dataReader["lg_straat"];
                        int huisNrG = (int)dataReader["lg_huisNr"];
                        Locatie locatie1 = new Locatie(postCodeG, gemeenteG, straatG, huisNrG);

                        // Gebruiker
                        int klantNr1 = (int)dataReader["klant_nr"];
                        string naam = (string)dataReader["naam"];
                        string email = (string)dataReader["g_email"];
                        string tel = (string)dataReader["telefoon_nr"];
                        Gebruiker gebruiker = new Gebruiker(klantNr1, naam, email, tel, locatie1);

                        // Tafel
                        int tafelId = (int)dataReader["tafel_id"];
                        int tafelPlaats = (int)dataReader["plaatsen"];


                        // Locatie - Restaurant
                        string postCode = (string)dataReader["lr_postcode"];
                        string gemeente = (string)dataReader["lr_gemeente"];
                        string straat = (string)dataReader["lr_straat"];
                        int huisNr = (int)dataReader["lr_huisNr"];
                        Locatie locatie2 = new Locatie(postCode, gemeente, straat, huisNr);

                        // Restaurant
                        int restaurantId = (int)dataReader["restaurant_id"];
                        string restoNaam = (string)dataReader["resto_naam"];
                        string keuken1 = (string)dataReader["keuken"];
                        string r_email = (string)dataReader["r_email"];
                        string telefoon = (string)dataReader["telefoon"];
                        Restaurant restaurant = new Restaurant(restaurantId, restoNaam, locatie2, keuken1, telefoon, r_email);


                        reservatie = new Reservatie(resNr, gebruiker, restaurant, aantalPlaatsen,
                            datum, new Tafel(tafelId, tafelPlaats));

                    }
                    dataReader.Close();
                    return reservatie;

                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException($"Reservatie met id: {reservatieNr} bestaat niet", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool HeeftReservatie(int reservatieNr)
        {
            string query = "SELECT count(*) FROM Reservatie where reservatie_nr=@reservatieNr";
            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@reservatieNr", reservatieNr);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    throw new ReservatieRepositoryException("HeeftReservatie", ex);
                }
                finally { con.Close(); }
            }
        }

        public void UpdateReservatie(Reservatie reservatie)
        {
            string query = "UPDATE Reservatie SET aantal_plaatsen=@aantalPlaatsen, datum=@datum, tafel_id=@tafelNr " +
                "WHERE reservatie_nr=@reservatieNr";
            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@reservatieNr", reservatie.ReservatieNr);
                    cmd.Parameters.AddWithValue("@aantalPlaatsen", reservatie.AantalPlaatsen);
                    cmd.Parameters.AddWithValue("@datum", reservatie.Datum);
                    cmd.Parameters.AddWithValue("@tafelNr", reservatie.Tafel.Id);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new ReservatieRepositoryException("UpdateReservatie", ex);
                }
                finally { con.Close(); }
            }
        }

        public void VerwijderReservatie(Reservatie reservatie)
        {
            string reservatieQuery = "DELETE FROM Reservatie WHERE reservatie_nr=@id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                try
                {
                    con.Open();
                    SqlCommand reservatieCmd = new SqlCommand(reservatieQuery, con);
                    reservatieCmd.Parameters.AddWithValue("@id", reservatie.ReservatieNr);
                    reservatieCmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new ReservatieRepositoryException("VerwijderReservatie", ex);
                }
                finally { con.Close(); }
            }
        }

        public List<Reservatie> GeefReservatieOpDatum(DateTime beginDatum, DateTime? eindDatum)
        {
            string query = "SELECT *, r.id as restaurant_id, r.naam as resto_naam, t.id as tafel_id, " +
                "lg.postcode as lg_postcode, lg.gemeente as lg_gemeente, lg.straat as lg_straat, lg.huisNr as lg_huisnr , " +
                "lr.postcode as lr_postcode, lr.gemeente as lr_gemeente, lr.straat as lr_straat, lr.huisNr as lr_huisnr, " +
                "g.email as g_email, r.email as r_email " +
                "FROM Reservatie res " +
                "join Gebruiker g on g.klant_nr=res.contact_persoon_id " +
                "join Restaurant r on r.id=res.restaurant_id  " +
                "join Tafel t on t.id = res.tafel_id " +
                "left join Locatie lg on g.locatie_id = lg.id " +
                "left join locatie lr on r.locatie_id = lr.id " +
                "where res.datum >= @beginDatum ";

            if (eindDatum != null)
            {
                query += "and res.datum <= @eindDatum";
            }
            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = query;

                    string sd = beginDatum.ToString("yyyy-MM-dd HH:mm:ss");
                    cmd.Parameters.AddWithValue("@beginDatum", sd);
                    if (eindDatum != null)
                    {
                        string ed = eindDatum.Value.ToString("yyyy-MM-dd HH:mm:ss");
                        cmd.Parameters.AddWithValue("@eindDatum", ed);
                    }

                    IDataReader dataReader = cmd.ExecuteReader();
                    List<Reservatie> reservaties = new List<Reservatie>();

                    while (dataReader.Read())
                    {
                        int resNr = (int)dataReader["reservatie_nr"];
                        int aantalPlaatsen = (int)dataReader["aantal_plaatsen"];
                        int tafelNr = (int)dataReader["tafel_id"];
                        DateTime datum = (DateTime)dataReader["datum"];

                        // Locatie - Gebruiker
                        string postCodeG = (string)dataReader["lg_postcode"];
                        string gemeenteG = (string)dataReader["lg_gemeente"];
                        string straatG = (string)dataReader["lg_straat"];
                        int huisNrG = (int)dataReader["lg_huisNr"];
                        Locatie locatie1 = new Locatie(postCodeG, gemeenteG, straatG, huisNrG);

                        // Gebruiker
                        int klantNr1 = (int)dataReader["klant_nr"];
                        string naam = (string)dataReader["naam"];
                        string email = (string)dataReader["g_email"];
                        string tel = (string)dataReader["telefoon_nr"];
                        Gebruiker gebruiker = new Gebruiker(klantNr1, naam, email, tel, locatie1);

                        // Tafel
                        int tafelId = (int)dataReader["tafel_id"];
                        int tafelPlaats = (int)dataReader["plaatsen"];


                        // Locatie - Restaurant
                        string postCode = (string)dataReader["lr_postcode"];
                        string gemeente = (string)dataReader["lr_gemeente"];
                        string straat = (string)dataReader["lr_straat"];
                        int huisNr = (int)dataReader["lr_huisNr"];
                        Locatie locatie2 = new Locatie(postCode, gemeente, straat, huisNr);

                        // Restaurant
                        int restaurantId = (int)dataReader["restaurant_id"];
                        string restoNaam = (string)dataReader["resto_naam"];
                        string keuken1 = (string)dataReader["keuken"];
                        string remail = (string)dataReader["r_email"];
                        string telefoon = (string)dataReader["telefoon"];
                        Restaurant restaurant = new Restaurant(restaurantId, restoNaam, locatie2, keuken1, telefoon, remail);


                        reservaties.Add(new Reservatie(resNr, gebruiker, restaurant, aantalPlaatsen,
                            datum, new Tafel(tafelId, tafelPlaats)));

                    }
                    dataReader.Close();
                    return reservaties;

                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException($"GeefReservatieOpDatum", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }


    }
}