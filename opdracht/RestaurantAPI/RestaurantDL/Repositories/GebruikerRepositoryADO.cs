using RestaurantBL.Interfaces;
using RestaurantBL.Models;
using RestaurantDL.Exceptions;
using System;
using System.Data;
using System.Data.SqlClient;

namespace RestaurantDL.Repositories
{
    public class GebruikerRepositoryADO : IGebruikerRepository
    {
        private string _connectionString;

        public GebruikerRepositoryADO(string connection)
        {
            _connectionString = connection;
        }
        public void UpdateGebruiker(Gebruiker gebruiker)
        {
            string gebruikerQuery = "UPDATE Gebruiker SET naam=@naam, email=@email, telefoon_nr=@tel WHERE klant_nr=@klantNr";
            string locatieQuery = "UPDATE Locatie SET postcode=@postcode, gemeente=@gemeente, straat=@straat, huisNr=@huisNr " +
                "where id = @locatieId";
            SqlTransaction trans = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    // Locatie
                    SqlCommand cmdL = new SqlCommand(locatieQuery, conn, trans);
                    cmdL.Parameters.AddWithValue("@postcode", gebruiker.Locatie.PostCode);
                    cmdL.Parameters.AddWithValue("@gemeente", gebruiker.Locatie.Gemeente);
                    cmdL.Parameters.AddWithValue("@huisNr", gebruiker.Locatie.HuisNr);
                    cmdL.Parameters.AddWithValue("@straat", gebruiker.Locatie.Straat);
                    cmdL.Parameters.AddWithValue("@locatieId", gebruiker.Locatie.Id);
                    cmdL.ExecuteNonQuery();

                    // Gebruiker
                    SqlCommand cmdG = new SqlCommand(gebruikerQuery, conn, trans);
                    cmdG.Parameters.AddWithValue("@klantNr", gebruiker.KlantNr);
                    cmdG.Parameters.AddWithValue("@naam", gebruiker.Naam);
                    cmdG.Parameters.AddWithValue("@email", gebruiker.Email);
                    cmdG.Parameters.AddWithValue("@tel", gebruiker.TelefoonNr);
                    cmdG.ExecuteNonQuery();

                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new GebruikerRepositoryException("UpdateGebruiker", ex);
                }
                finally { conn.Close(); }
            }
        }
        public void VoegGebruikerToe(Gebruiker gebruiker)
        {
            string gebruikerQuery = "insert into Gebruiker(naam, email, telefoon_nr, locatie_id) " +
                "output INSERTED.klant_nr " +
                "VALUES(@naam, @email, @telefoon, @locatie )";

            string locatieQuery = "insert into Locatie(postcode, gemeente, straat, huisnr) output INSERTED.id " +
                "VALUES(@postcode, @gemeente, @straat, @huisnr)";

            SqlTransaction trans = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try 
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    // Locatie
                    SqlCommand cmdL = new SqlCommand(locatieQuery, conn, trans);
                    cmdL.CommandText = locatieQuery;
                    cmdL.Parameters.AddWithValue("@postcode", gebruiker.Locatie.PostCode);
                    cmdL.Parameters.AddWithValue("@gemeente", gebruiker.Locatie.Gemeente);
                    cmdL.Parameters.AddWithValue("@straat", gebruiker.Locatie.Straat);
                    cmdL.Parameters.AddWithValue("@huisnr", gebruiker.Locatie.HuisNr);
                    gebruiker.Locatie.Id =  (int)cmdL.ExecuteScalar();

                    // Gebruiker
                    SqlCommand cmdG = new SqlCommand(gebruikerQuery, conn, trans);
                    cmdG.CommandText = gebruikerQuery;
                    cmdG.Parameters.AddWithValue("@naam", gebruiker.Naam);
                    cmdG.Parameters.AddWithValue("@email", gebruiker.Email);
                    cmdG.Parameters.AddWithValue("@telefoon", gebruiker.TelefoonNr);
                    cmdG.Parameters.AddWithValue("@locatie", gebruiker.Locatie.Id);
                    int klantNr = (int)cmdG.ExecuteScalar();
                    gebruiker.KlantNr = klantNr;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new GebruikerRepositoryException("VoegGebruikerToe", ex);
                }
                finally { conn.Close(); }
            }
        }


        public Gebruiker GeefGebruiker(int klantNr)
        {
            string query = "SELECT * FROM Gebruiker g join Locatie l on g.locatie_id = l.id where klant_nr=@klantNr and g.actief = 1";
            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantNr", klantNr);
                    IDataReader dataReader = cmd.ExecuteReader();
                    Gebruiker gebruiker = null;
                    if (dataReader.Read())
                    {

                        // Locatie
                        string postCode = (string)dataReader["postcode"];
                        string gemeente = (string)dataReader["gemeente"];
                        string straat = (string)dataReader["straat"];
                        int huisNr = (int)dataReader["huisNr"];
                        int lid = (int)dataReader["id"];
                        Locatie locatie = new Locatie(lid, postCode, gemeente, straat, huisNr);

                        int klantNr1 = (int)dataReader["klant_nr"];
                        string naam = (string)dataReader["naam"];
                        string email = (string)dataReader["email"];
                        string tel = (string)dataReader["telefoon_nr"];
                        gebruiker = new Gebruiker(klantNr1, naam, email, tel, locatie);
                        dataReader.Close();
                    }


                    return gebruiker;
                }
                catch (Exception ex)
                {
                    throw new GebruikerRepositoryException("GeefGebruiker", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool HeeftGebruiker(int klantNr)
        {
            string query = "SELECT count(*) FROM Gebruiker where klant_nr=@klantNr and actief = 1";
            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantNr", klantNr);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    throw new GebruikerRepositoryException("HeeftGemeente", ex);
                }
                finally { con.Close(); }
            }
        }

        public void VerwijderGebruiker(int klantNr, int locatieId)
        {
            string gebruikerQuery = "UPDATE Gebruiker SET actief=0 WHERE klant_nr=@klantNr";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();

                    SqlCommand cmdG = new SqlCommand(gebruikerQuery, conn);
                    cmdG.Parameters.AddWithValue("@klantNr", klantNr);
                    cmdG.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    
                    throw new GebruikerRepositoryException("VerwijderGebruiker", ex);
                }
                finally { conn.Close(); }
            }
        }

        private static Locatie DataReaderLeesLocatie(IDataReader dataReader)
        {
            // Locatie
            int id = (int)dataReader["locatie_id"];
            string postCode = (string)dataReader["postcode"];
            string gemeente = (string)dataReader["gemeente"];
            string straat = (string)dataReader["straat"];
            int huisNr = (int)dataReader["huisNr"];
            Locatie l = new Locatie(id, postCode, gemeente, straat, huisNr);
            return l;
        }
        public Locatie GeefLocatie(int id)
        {
            string restaurantQuery = "SELECT *, l.id as locatie_id from Gebruiker g join locatie l on l.id = g.locatie_id " +
                "where g.klant_nr = @id";

            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = restaurantQuery;
                    cmd.Parameters.AddWithValue("@id", id);
                    IDataReader dataReader = cmd.ExecuteReader();

                    Locatie locatie = null;

                    if (dataReader.Read())
                    {
                        locatie = DataReaderLeesLocatie(dataReader);
                    }
                    dataReader.Close();
                    return locatie;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException($"Gebruiker met id: {id} kan niet worden ophaald", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
