using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace dm_backend.Models
{
    public class Permission
    {
        public int? PermissionId { get; set; }
        public string PermissionName { get; set; }

        internal AppDb Db { get; set; }
        public Permission(){

        }
        public Permission(AppDb db){
            Db = db;
        }
        public int DeletePermission()
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM permission WHERE  NOT EXISTS
(SELECT * from role_to_permission  WHERE  role_to_permission.permission_id = permission.permission_id)
 and permission_id=@permission_id;";
            BindPermissionId(cmd);
            int numberOfRecords=  cmd.ExecuteNonQuery();
            Db.Connection.Close();
           Console.WriteLine(numberOfRecords);
           if(numberOfRecords>0)
           {
               Console.WriteLine("permission deleted");
               return 1;
           }
           else {
               Console.WriteLine("permission not deleted");
               return 0;
               }
        }
        public void AddPermission()
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "insert into permission (permission_name) values(@permission_name)";
            BindPermissionName(cmd);
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
        public void UpdatePermission()
        {
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"update permission set permission_name=@permission_name where permission_id=@permission_id";
            BindPermission(cmd);
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
        private void BindPermissionId(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("@permission_id", PermissionId));
        }
        private void BindPermissionName(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("@permission_name", PermissionName));
        }
        private void BindPermission(MySqlCommand cmd)
        {
            BindPermissionId(cmd);
            BindPermissionName(cmd);
        }
    }
}
