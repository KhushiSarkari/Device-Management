using System;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using dm_backend.Models;
namespace dm_backend.Logics
{
    public class GetAllAdmin
    {

        public AppDb Db { get; set; }

        public GetAllAdmin(AppDb db)
        {
            Db = db;
        }

        
        public List<Request> getAllAdmin()
        {


            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "get_all_admin";
            cmd.CommandType = CommandType.StoredProcedure;

            try
            {
                return (BindRequestData(cmd.ExecuteReader()));
            }
            catch
            {
                return null;
            }
            
        }


     


        public List<Request> BindRequestData(DbDataReader reader)
        {
            using (reader)
            {
                var model = new List<Request>();
                while (reader.Read())
                {
                    model.Add(new Request()
                    {
                        name = (reader.GetString("salutation") + " " + reader.GetString("first_name") + " " + (reader.IsDBNull("middle_name") ? "" : (reader.GetString("middle_name") + " ")) + reader.GetString("last_name")),
                        email = reader.GetString("email")
                    });
                }
                return model;
            }
        }

    }
}
