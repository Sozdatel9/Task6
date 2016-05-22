using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.DalContracts;
// using Task6.BllContracts;
using Task6.Entities;
using Task6.DBDal;

namespace Task6.Logic
{
    public class DalProvider
    {
        public static readonly string ConnectionString;

        public static IUserDao UserDao
        {
            get; set;
        }

        public static IAwardDao AwardDao
        {
            get; set;
        }

        public static IAccountDao AccountDao
        {
            get; set;
        }

        public static IRoleDao RoleDao
        {
            get; set;
        }

        static DalProvider()
        {
            string dalType = ConfigurationManager.AppSettings["DalType"];
            var connectionString = ConfigurationManager.ConnectionStrings["default"].ConnectionString;
            switch (dalType.ToLower())
            {
                case "db":
                    AwardDao = new DbAwardDao();
                    UserDao = new DbUserDao();
                    AccountDao = new DbAccountDao();
                    RoleDao = new DbRoleDao();
                    ConnectionString = connectionString;
                    break;

                default:
                    throw new ConfigurationErrorsException($"Invalid dalType: {dalType}");
            }
        }
    }
}
