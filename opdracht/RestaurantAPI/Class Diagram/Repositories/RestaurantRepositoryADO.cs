using RestaurantBL.Interfaces;
using RestaurantBL.Models;
using RestaurantDL.Exceptions;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Formats.Asn1.AsnWriter;

namespace RestaurantDL.Repositories
{

    public class RestaurantRepositoryADO : IRestaurantRepository
    {
        private string _connectionString;
        private LocatieRepositoryADO _locatieRepositoryADO;

        public RestaurantRepositoryADO(string connection)
        {
            _connectionString = connection;
            _locatieRepositoryADO = new LocatieRepositoryADO(connection);


        }
        public void VoegRestaurantToe(Restaurant restaurant)
        {
            string restauratnQuery = "insert into Restaurant(naam, keuken, telefoon, email, locatie_id) " +
                "output INSERTED.id VALUES(@naam, @keuken, @telefoon, @email, @locatieId); ";

            string tafelQuery = "insert into Tafel(plaatsen) " +
                "output INSERTED.id values(@plaatsen);";

            string tafelRestoQuery = "insert into Restaurant_tafel(restaurant_id, tafel_id) " +
                "values (@restaurantId, @tafelId);";

            string locatieQuery = "insert into Locatie(postcode, gemeente, straat, huisnr) output INSERTED.id " +
                "VALUES(@postcode, @gemeente, @straat, @huisnr)";

            SqlTransaction trans = null;


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                trans = conn.BeginTransaction();



                try
                {
                    // Locatie
                    SqlCommand cmdL = new SqlCommand(locatieQuery, conn, trans);
                    cmdL.CommandText = locatieQuery;
                    cmdL.Parameters.AddWithValue("@postcode", restaurant.Locatie.PostCode);
                    cmdL.Parameters.AddWithValue("@gemeente", restaurant.Locatie.Gemeente);
                    cmdL.Parameters.AddWithValue("@straat", restaurant.Locatie.Straat);
                    cmdL.Parameters.AddWithValue("@huisnr", restaurant.Locatie.HuisNr);
                    restaurant.Locatie.Id = (int)cmdL.ExecuteScalar();


                    // Restaurant
                    SqlCommand restaurantCmd = new SqlCommand(restauratnQuery, conn, trans);
                    restaurantCmd.Parameters.AddWithValue("@naam", restaurant.Naam);
                    restaurantCmd.Parameters.AddWithValue("@keuken", restaurant.Keuken);
                    restaurantCmd.Parameters.AddWithValue("@locatieId", restaurant.Locatie.Id);
                    restaurantCmd.Parameters.AddWithValue("@telefoon", restaurant.Telefoon);
                    restaurantCmd.Parameters.AddWithValue("@email", restaurant.Email);
                    restaurant.Id = (int)restaurantCmd.ExecuteScalar();


                    // Tafel
                    foreach (Tafel tafel in restaurant.Tafels)
                    {
                        SqlCommand tafelCmd = new SqlCommand(tafelQuery, conn, trans);
                        SqlCommand restaurantTafel = new SqlCommand(tafelRestoQuery, conn, trans);
                        tafelCmd.Parameters.AddWithValue("@plaatsen", tafel.Plaatsen);
                        tafel.Id = (int)tafelCmd.ExecuteScalar();

                        // Restaurant - Tafel
                        restaurantTafel.Parameters.AddWithValue("@restaurantId", restaurant.Id);
                        restaurantTafel.Parameters.AddWithValue("@tafelId", tafel.Id);
                        restaurantTafel.ExecuteNonQuery();
                    }
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    // Log error      
                    trans.Rollback();
                    throw new RestaurantRepositoryException("VoegRestaurantToe", ex);
                }
                finally { conn.Close(); }
            }
        }

        public void UpdateRestaurant(Restaurant restaurant)
        {

            string query = "UPDATE Restaurant " +
                "SET naam=@naam, keuken=@keuken, telefoon=@telefoon, email=@email " +
                "WHERE id=@id";

            string locatieQuery = "UPDATE Locatie SET postcode=@postcode, gemeente=@gemeente, straat=@straat, huisNr=@huisNr " +
                "where id = @locatieId";

            string tafelQuery = "UPDATE Tafel SET plaatsen = @plaatsen WHERE id=@tafelId";

            SqlTransaction trans = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();

                    // Restaurant
                    SqlCommand cmdR = new SqlCommand(query, conn, trans);
                    cmdR.Parameters.AddWithValue("@id", restaurant.Id);
                    cmdR.Parameters.AddWithValue("@naam", restaurant.Naam);
                    cmdR.Parameters.AddWithValue("@keuken", restaurant.Keuken);
                    cmdR.Parameters.AddWithValue("@telefoon", restaurant.Telefoon);
                    cmdR.Parameters.AddWithValue("@email", restaurant.Email);
                    cmdR.ExecuteNonQuery();

                    // Locatie
                    SqlCommand cmdL = new SqlCommand(locatieQuery, conn, trans);
                    cmdL.Parameters.AddWithValue("@postcode", restaurant.Locatie.PostCode);
                    cmdL.Parameters.AddWithValue("@gemeente", restaurant.Locatie.Gemeente);
                    cmdL.Parameters.AddWithValue("@huisNr", restaurant.Locatie.HuisNr);
                    cmdL.Parameters.AddWithValue("@straat", restaurant.Locatie.Straat);
                    cmdL.Parameters.AddWithValue("@locatieId", restaurant.Locatie.Id);
                    cmdL.ExecuteNonQuery();

                    // Tafel
                    if(restaurant.Tafels != null)
                    {
                        foreach (Tafel tafel in restaurant.Tafels)
                        {
                            SqlCommand cmdT = new SqlCommand(tafelQuery, conn, trans);
                            cmdT.Parameters.AddWithValue("@tafelId", tafel.Id);
                            cmdT.Parameters.AddWithValue("@plaatsen", tafel.Plaatsen);
                            cmdT.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();

                    throw new RestaurantRepositoryException("UpdateRestaurant", ex);
                }
                finally { conn.Close(); }
            }
        }

        public List<Restaurant> GeefRestaurantOpLocatieKeuken(Locatie locatie, string keuken)
        {
            string query = "SELECT * FROM Restaurant r " +
                "join locatie l on l.id = r.locatie_id " +
                "where l.postcode=@postcode and keuken=@keuken and r.actief = 1";

            //string tafelQuery = "SELECT * FROM Restaurant_tafel rt " +
            //    "join Tafel t on rt.tafel_id = t.id " +
            //    "where rt.restaurant_id = @tafelId";

            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@postcode", locatie.PostCode);
                    cmd.Parameters.AddWithValue("@keuken", keuken);
                    IDataReader dataReader = cmd.ExecuteReader();
                    List<Restaurant> restaurants = new List<Restaurant>();
                    while (dataReader.Read())
                    {
                        Locatie l = DataReaderLeesLocatie(dataReader);

                        int resId = (int)dataReader["id"];
                        string naam = (string)dataReader["naam"];
                        string keuken1 = (string)dataReader["keuken"];
                        string tel = (string)dataReader["telefoon"];
                        string email = (string)dataReader["email"];
                        restaurants.Add(new Restaurant(resId, naam, l, keuken1, tel, email));
                    }
                    dataReader.Close();

                    foreach (Restaurant restaurant in restaurants)
                    {
                        StelTafelsInRestaurant(restaurant);
                        StelReservatiesInRestaurant(restaurant);
                        //cmd.CommandText = tafelQuery;
                        //cmd.Parameters.AddWithValue("@tafelId", restaurant.Id);
                        //IDataReader dataReader2 = cmd.ExecuteReader();

                        //List<Tafel> tafels = new List<Tafel>();
                        //while (dataReader2.Read())
                        //{
                        //    int tafelId = (int)dataReader2["id"];
                        //    int aantal = (int)dataReader2["plaatsen"];
                        //    bool vrij = (bool)dataReader2["vrij"];

                        //    tafels.Add(new Tafel(tafelId, aantal));
                        //}
                        //dataReader2.Close();
                        //restaurant.Tafels = tafels;
                    }


                    return restaurants;


                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GeefRestaurant", ex);
                }
                finally
                {
                    conn.Close();
                }
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

        public Restaurant GeefRestaurant(int id)
        {
            string restaurantQuery = "SELECT *, l.id as locatie_id FROM Restaurant r " +
                "join locatie l on l.id = r.locatie_id " +
                "where r.id=@id and r.actief = 1";

            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = restaurantQuery;
                    cmd.Parameters.AddWithValue("@id", id);
                    IDataReader dataReader = cmd.ExecuteReader();

                    Restaurant restaurant = null;
                    int restaurantId = 0;
                    int restaurantTafelId = 0;
                    if (dataReader.Read())
                    {
                        Locatie l = DataReaderLeesLocatie(dataReader);
                        // Restaurant
                        restaurantId = (int)dataReader["id"];
                        string naam = (string)dataReader["naam"];
                        string keuken1 = (string)dataReader["keuken"];
                        string telefoon = (string)dataReader["telefoon"];
                        string email = (string)dataReader["email"];



                        restaurant = new Restaurant(restaurantId, naam, l, keuken1, telefoon, email);

                    }
                    dataReader.Close();
                    if (restaurant != null)
                    {
                        StelTafelsInRestaurant(restaurant);
                        StelReservatiesInRestaurant(restaurant);
                    }

                    return restaurant;

                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException($"Restaurant met id: {id} kan niet worden ophaald", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void StelReservatiesInRestaurant(Restaurant restaurant)
        {
            
                string reservatieQuery = "SELECT *, t.id as tafel_id FROM Restaurant r " +
                "join reservatie reservatie on reservatie.restaurant_id = r.id " +
                "join gebruiker g on reservatie.contact_persoon_id = g.klant_nr " +
                "join tafel t on t.id = reservatie.tafel_id " +
                "join locatie l on l.id = g.locatie_id " +
                "where r.id=@restaurantId ";

                SqlConnection conn = new SqlConnection(_connectionString);
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = reservatieQuery;
                        cmd.Parameters.AddWithValue("@restaurantId", restaurant.Id);
                        IDataReader dataReader = cmd.ExecuteReader();
                        List<Reservatie> reservaties = new List<Reservatie>();
                        while (dataReader.Read())
                        {
                            Locatie l = DataReaderLeesLocatie(dataReader);

                            // Gebruiker
                            int klantNr1 = (int)dataReader["klant_nr"];
                            string naamGebruiker = (string)dataReader["naam"];
                            string email = (string)dataReader["email"];
                            string tel = (string)dataReader["telefoon_nr"];
                            Gebruiker gebruiker = new Gebruiker(klantNr1, naamGebruiker, email, tel, l);

                            // Tafel
                            int tafelId = (int)dataReader["tafel_id"];
                            int tafelPlaats = (int)dataReader["plaatsen"];

                            // Reservatie
                            int reservatieId = (int)dataReader["reservatie_nr"];
                            int plaatsen = (int)dataReader["aantal_plaatsen"];
                            int tafelNr = (int)dataReader["id"];
                            DateTime datum = (DateTime)dataReader["datum"];

                            reservaties.Add(new Reservatie(reservatieId, gebruiker, restaurant, plaatsen, datum,
                                new Tafel(tafelId, tafelPlaats)));

                        }
                        restaurant.Reservaties = reservaties;
                        dataReader.Close();
                    }
                    catch (Exception ex)
                    {
                        throw new RestaurantRepositoryException("Kan reservatie niet ophalen", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }

                

            }
        }

        private void StelTafelsInRestaurant(Restaurant restaurant)
        {
           
                string tafelQuery = "SELECT * FROM Restaurant_tafel rt " +
                "join Tafel t on rt.tafel_id = t.id " +
                "where rt.restaurant_id = @tafelId and t.actief = 1";

                SqlConnection conn = new SqlConnection(_connectionString);
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    try
                    {
                        conn.Open();
                        cmd.CommandText = tafelQuery;
                        cmd.Parameters.AddWithValue("@tafelId", restaurant.Id);
                        IDataReader dataReader = cmd.ExecuteReader();

                        List<Tafel> tafels = new List<Tafel>();
                        while (dataReader.Read())
                        {
                            int tafelId = (int)dataReader["id"];
                            int aantal = (int)dataReader["plaatsen"];

                            tafels.Add(new Tafel(tafelId, aantal));
                        }
                        dataReader.Close();
                        restaurant.Tafels = tafels;
                    }
                    catch (Exception ex)
                    {
                        throw new RestaurantRepositoryException("GeefRestaurant", ex);
                    }
                    finally
                    {
                        conn.Close();
                    }
                
            }
        }

        public void VerwijderRestaurant(Restaurant restaurant)
        {
            string restaurantQuery = "UPDATE Restaurant SET actief=0 WHERE id=@id";
            //string tafelQuery = "DELETE FROM Tafel WHERE id=@id";
            //string restaurantTafelQuery = "DELETE FROM Restaurant_tafel output DELETED.tafel_id " +
            //    "WHERE restaurant_id=@restaurant_id ";
            //string locatieQuery = "DELETE FROM Locatie WHERE id=@locatieId";
            //string reservatieQuery = "DELETE FROM Reservatie where restaurant_id = @restaurantId;";

            SqlTransaction trans = null;
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                trans = con.BeginTransaction();
                SqlCommand restaurantCmd = new SqlCommand(restaurantQuery, con, trans);
                //SqlCommand tafelCmd = new SqlCommand(tafelQuery, con, trans);
                //SqlCommand restaurantTafelCmd = new SqlCommand(restaurantTafelQuery, con, trans);
                //SqlCommand locatieCmd = new SqlCommand(locatieQuery, con, trans);
                //SqlCommand reseratieCmd = new SqlCommand(reservatieQuery, con, trans);

                try
                {
                    //if (restaurant.Tafels.Count > 0)
                    //{
                    //    restaurantTafelCmd.Parameters.AddWithValue("@restaurant_id", restaurant.Id);
                    //    int tafelId = (int)restaurantTafelCmd.ExecuteScalar();

                    //    tafelCmd.Parameters.AddWithValue("@id", tafelId);
                    //    restaurantTafelCmd.ExecuteNonQuery();
                    //}

                    //reseratieCmd.Parameters.AddWithValue("@restaurantId", restaurant.Id);
                    //reseratieCmd.ExecuteNonQuery();

                    restaurantCmd.Parameters.AddWithValue("@id", restaurant.Id);
                    restaurantCmd.ExecuteNonQuery();

                    //locatieCmd.Parameters.AddWithValue("@locatieId", restaurant.Locatie.Id);
                    //locatieCmd.ExecuteNonQuery();



                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new RestaurantRepositoryException("VerwijderRestaurant", ex);
                }
                finally { con.Close(); }
            }
        }

        public bool HeefRestaurant(int id)
        {
            string query = "select count(*) from Restaurant where id=@id and actief = 1";
            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@id", id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("HeeftRestaurant", ex);
                }
                finally { con.Close(); }
            }
        }

        public void VoegTafelsToe(int id, List<Tafel> tafels)
        {
            //string query = "UPDATE Restaurant " +
            //    "SET naam=@naam, keuken=@keuken, locatie=@locatie, contact_gegevens=@contact_gegevens " +
            //    "WHERE id=@id";

            string tafelSql = "insert into Tafel(plaatsen) " +
                "output INSERTED.id values(@plaatsen);";

            string tafelRestoSql = "insert into Restaurant_tafel(restaurant_id, tafel_id) " +
                "values (@restaurantId, @tafelId);";

            SqlTransaction trans = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    trans = conn.BeginTransaction();
                    // Tafel
                    foreach (Tafel tafel in tafels)
                    {
                        SqlCommand tafelCmd = new SqlCommand(tafelSql, conn, trans);
                        SqlCommand restaurantTafel = new SqlCommand(tafelRestoSql, conn, trans);
                        tafelCmd.Parameters.AddWithValue("@plaatsen", tafel.Plaatsen);
                        int tafelId = (int)tafelCmd.ExecuteScalar();

                        // Restaurant - Tafel
                        restaurantTafel.Parameters.AddWithValue("@restaurantId", id);
                        restaurantTafel.Parameters.AddWithValue("@tafelId", tafelId);
                        restaurantTafel.ExecuteNonQuery();
                    }
                    trans.Commit();

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw new RestaurantRepositoryException("UpdateRestaurant", ex);
                }
                finally { conn.Close(); }
            }
        }

        public Tafel GeefTafel(int id)
        {
            string query = "SELECT * FROM Tafel where id=@id";

            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@id", id);
                    IDataReader dataReader = cmd.ExecuteReader();
                    Tafel tafel = null;
                    while (dataReader.Read())
                    {
                        int plaatsen = (int)dataReader["plaatsen"];
                        tafel = new Tafel(id, plaatsen);
                    }
                    dataReader.Close();
                    return tafel;


                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("GeefTafel", ex);
                }

                finally
                {
                    conn.Close();
                }
            }
        }

        public Locatie GeefLocatie(int id)
        {
            string restaurantQuery = "SELECT *, l.id as locatie_id from Restaurant r join locatie l on l.id = r.locatie_id where r.id = @id";

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
                    throw new RestaurantRepositoryException($"Restaurant met id: {id} kan niet worden ophaald", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public Locatie GeefLocatieOpPostCode(string postcode)
        {
            string restaurantQuery = "SELECT *, l.id as locatie_id from Restaurant r " +
                "join locatie l on l.id = r.locatie_id " +
                "where l.postcode = @postcode";

            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = restaurantQuery;
                    cmd.Parameters.AddWithValue("@postcode", postcode);
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
                    throw new RestaurantRepositoryException($"Locaite met gemeente: {postcode} kan niet worden ophaald", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        public bool HeeftTafel(int id)
        {
            string query = "select count(*) from Tafel where id=@id";
            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@id", id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("HeeftTafel", ex);
                }
                finally { con.Close(); }
            }
        }

        public void VerwijderTafel(int tafelId)
        {
            string tafelQuery = "UPDATE Tafel SET actief=0 WHERE id=@id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                con.Open();
                try
                {
                    SqlCommand tafelCmd = new SqlCommand(tafelQuery, con);
                    tafelCmd.Parameters.AddWithValue("@id", tafelId);
                    tafelCmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException("VerwijderTafel", ex);
                }
                finally { con.Close(); }
            }
        }

        public Restaurant GeefRestaurantOpTafelId(int tafelId)
        {
            string query = "select * from restaurant_tafel where tafel_id=@id";
            SqlConnection conn = new SqlConnection(_connectionString);
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@id", tafelId);
                    IDataReader dataReader = cmd.ExecuteReader();

                    Restaurant restaurant = null;
                    int restaurantId = 0;
                    if (dataReader.Read())
                    {
                        restaurantId = (int)dataReader["restaurant_id"];
                        
                    }
                    dataReader.Close();
                    
                    if(restaurantId > 0)
                    {
                        restaurant = GeefRestaurant(restaurantId);
                    }

                    return restaurant;

                }
                catch (Exception ex)
                {
                    throw new RestaurantRepositoryException($"Restaurant niet gevonden", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
