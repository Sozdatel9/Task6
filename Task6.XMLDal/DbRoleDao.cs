using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Task6.DalContracts;
using Task6.Entities;
using Task6.Logic;

namespace Task6.DBDal
{
    public class DbRoleDao : IRoleDao
    {
        public DbRoleDao()
        {
        }

        public string[] GetAllRoles()
        {
            return new[] { "user", "admin" };
        }

        public string[] GetRolesForUser(string userName)
        {
            if (IsUserInRole(userName, "admin"))
            {
                return new[] { "admin" };
            }
            else if (IsUserInRole(userName, "user"))
            {
                return new[] { "user" };
            }
            else return new[] { "null" };
        }

        public bool IsUserInRole(string userName, string Permissions)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdIsUserInRole = connection.CreateCommand();
                cmdIsUserInRole.CommandText = $"SELECT Id FROM dbo.Accounts WHERE Name = @Name AND IsAdmin = @Permissions";
                cmdIsUserInRole.Parameters.AddWithValue("@Name", userName);
                cmdIsUserInRole.Parameters.AddWithValue("@Permissions", Permissions);
                connection.Open();
                var result = cmdIsUserInRole.ExecuteScalar();
                if (result != null)
                {
                    Result = true;
                }
                else { Result = false; }
            }
            return Result;
        }
    }
}
