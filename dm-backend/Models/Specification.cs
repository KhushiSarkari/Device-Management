using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using dm_backend;
using dm_backend.Utilities;

namespace dm_backend.Models
{
    public class Specification
    {
        public int specification_id { get; set; }

        public string RAM { get; set; }
        public string Storage { get; set; }
        public string ScreenSize { get; set; }

        public string Connectivity { get; set; }

        internal AppDb Db { get; set; }

        public Specification()
        {
        }

        internal Specification(AppDb db)
        {
            Db = db;
        }

        public string GetSafeString(DbDataReader reader, string colName)
        {
            return reader[colName] != DBNull.Value ? (string)reader[colName] : "";
        }

        public List<Specification> getAllSpecifications()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from specification;";

            return ReadSpecifications(cmd.ExecuteReader());
        }
        public List<Specification> getspecbyid(int specification_id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select * from specification where specification.specification_id=@specification_id;";

            cmd.Parameters.AddWithValue("@specification_id", specification_id);
            return ReadSpecifications(cmd.ExecuteReader());
        }

        public List<Specification> getSpecificSpecification(string typeId, string brand, string model)
        {

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"select s.* from specification as s inner join device as d using(specification_id)
inner join device_type as dt using (device_type_id)
inner join device_brand as db using (device_brand_id) 
inner join device_model as dm using (device_model_id)
where db.brand = @brand  and dt.type = @type  and dm.model=@model group by s.specification_id; ";

            cmd.Parameters.AddWithValue("@type", typeId);
            cmd.Parameters.AddWithValue("@brand", brand);
            cmd.Parameters.AddWithValue("@model", model);
            return ReadSpecifications(cmd.ExecuteReader());
        }


        public List<Specification> ReadSpecifications(DbDataReader reader)
        {
            var specifications = new List<Specification>();
            using (reader)
            {
                while (reader.Read())
                {
                    var spec1 = new Specification()
                    {
                        specification_id = reader.GetInt32(0),
                        RAM = GetSafeString(reader, "RAM"),
                        Storage = GetSafeString(reader, "storage"),
                        ScreenSize = GetSafeString(reader, "screen_size"),
                        Connectivity = GetSafeString(reader, "connectivity")


                    };
                    specifications.Add(spec1);
                }
            }
            return specifications;
        }
        async public Task addspecification(Specification s)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "addSpecification";
            cmd.CommandType = CommandType.StoredProcedure;
            Bindspec(cmd, s);
            await cmd.ExecuteNonQueryAsync();
        }
        private void Bindspec(MySqlCommand cmd, Specification s)
        {
            cmd.Parameters.Add(new MySqlParameter("RAM", s.RAM));
            cmd.Parameters.Add(new MySqlParameter("storage", s.Storage));
            cmd.Parameters.Add(new MySqlParameter("screen_size", s.ScreenSize));
            cmd.Parameters.Add(new MySqlParameter("connectivity", s.Connectivity));
        }
        async public Task updatespecification(Specification s1)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "updatespecification";
            cmd.CommandType = CommandType.StoredProcedure;
            Bindspec(cmd, s1);
            BindSpecificationId(cmd,s1);
            await cmd.ExecuteNonQueryAsync();
        }
        private void BindSpecificationId(MySqlCommand cmd, Specification s1)
        {
            cmd.Parameters.Add(new MySqlParameter("specification_id", s1.specification_id));
        }
        // Returns Specification ID from given specifications
        // Otherwise throws a NullReferenceException
        public int GetSpecificationID(AppDb db)
        {
            using var cmd = db.Connection.CreateCommand();

            cmd.CommandText = "get_specification_id";
            cmd.CommandType = CommandType.StoredProcedure;
            BindSpecificationParams(cmd);
            MySqlParameter SpecificationID = new MySqlParameter("@output", MySqlDbType.Int32) { Direction = ParameterDirection.ReturnValue };
            cmd.Parameters.Add(SpecificationID);
            cmd.ExecuteScalar();

            if (SpecificationID.Value == DBNull.Value)
            {
                throw new NullReferenceException("Specifications are not valid");
            }
            return (int)SpecificationID.Value;
        }

        private void BindSpecificationParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("var_ram", RAM));
            cmd.Parameters.Add(new MySqlParameter("var_storage", Storage));
            cmd.Parameters.Add(new MySqlParameter("var_screen_size", ScreenSize));
            cmd.Parameters.Add(new MySqlParameter("var_connectivity", Connectivity));
        }

    }
    }
