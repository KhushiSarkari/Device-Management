using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace dm_backend.Models
{
    public class Role
    {
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public List<Permission>? Permissions { get; set; }
        internal AppDb Db { get; set; }
        public Role()
        {

        }
        internal Role(AppDb db)
        {
            Db = db;
        }
        public Role GetRoleById(int id)
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = $@"select JSON_OBJECT(
                    'roleId', role_id,
                    'roleName', role_name,
                    'permissions', (select JSON_ARRAYAGG(
                        JSON_OBJECT(
                            #'permissionId', permission_id,
                            'permissionName', permission_name
                        )) from permission inner join role_to_permission using(permission_id) where role_to_permission.role_id=role_id
                    )
                ) as result from role where role_id={id}";
            using var reader = cmd.ExecuteReader();
            reader.Read();
            string result = reader.GetString("result");
            Role abc = JsonConvert.DeserializeObject<Role>(result);
            Db.Connection.Close();
            return abc;
        }
        public int Deleterole()
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM role WHERE  NOT EXISTS
           (SELECT * from user_to_role  WHERE  user_to_role.role_id = role.role_id) and role_id=@role_id;";
           BindRoleId(cmd);
           int numberOfRecords=  cmd.ExecuteNonQuery();
           Db.Connection.Close();
           Console.WriteLine(numberOfRecords);
           if(numberOfRecords>0)
           {
               Console.WriteLine("role deleted");
               return 1;
           }
           else {
               Console.WriteLine("role not deleted");
               return 0;
               }
            
            
        }
        public void AddRole()
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"insert into role(role_name) values(@role_name);";
            BindRoleName(cmd);
            try{
                cmd.ExecuteNonQuery();
            }
            catch(Exception){
                throw;
            }
            finally{
                Db.Connection.Close();
            }
        }
        public Role UpdateRole()
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"update role set role_name=@role_name where role_id=@role_id";
            BindRole(cmd);
            try
            {
                cmd.ExecuteNonQuery();
                return this;
            }
            catch(Exception){
                throw;
            }
            finally{
                Db.Connection.Close();
            }
        }
        private void BindRoleId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("role_id", RoleId));
        }
        private void BindRoleName(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("@role_name", RoleName));
        }
        private void BindRole(MySqlCommand cmd)
        {
            BindRoleId(cmd);
            BindRoleName(cmd);
        }
    }
}
