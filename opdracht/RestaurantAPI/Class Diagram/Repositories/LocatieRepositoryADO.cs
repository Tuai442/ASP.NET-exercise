using RestaurantBL.Interfaces;
using RestaurantBL.Models;
using RestaurantDL.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDL.Repositories
{
    public class LocatieRepositoryADO
    {
        private string _connectionString;

        public LocatieRepositoryADO(string connection)
        {
            _connectionString = connection;
        }


        public void VoeglocatieToe(Locatie locatie)
        {
            string sql = "insert into Locatie(postcode, gemeente, straat, huisnr) output INSERTED.id " +
                "VALUES(@postcode, @gemeente, @straat, @huisnr)";
            SqlConnection con = new SqlConnection(_connectionString);
            using (SqlCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@postcode", locatie.PostCode);
                    cmd.Parameters.AddWithValue("@gemeente", locatie.Gemeente);
                    cmd.Parameters.AddWithValue("@straat", locatie.Straat);
                    cmd.Parameters.AddWithValue("@huisnr", locatie.HuisNr);

                    int id = (int)cmd.ExecuteScalar();
                    locatie.Id = id;
                }
                catch (Exception ex)
                {
                    throw new LocatieRepositoryException("VoeglocatieToe", ex);
                }
                finally { con.Close(); }
            }
        }


    }
}
