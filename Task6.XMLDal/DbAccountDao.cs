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
    public class DbAccountDao : IAccountDao
    {
        HashSet<Account> Accounts;

        public DbAccountDao()
        {
        }

        private string Crypt(string input)
        {
            byte[] hash;
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }

        public bool Create(Account account)
        {
            Accounts = new HashSet<Account> { };
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAddAccount = connection.CreateCommand();
                var cmdNextAccountId = connection.CreateCommand();
                cmdAddAccount.CommandText = $"INSERT INTO dbo.Accounts (Name, Password, IsAdmin) VALUES (@Name, @Password, @Permissions)";
                cmdNextAccountId.CommandText = "SELECT @@IDENTITY";
                cmdAddAccount.Parameters.AddWithValue("@Name", account.Name);
                cmdAddAccount.Parameters.AddWithValue("@Password", Crypt(account.Password));
                cmdAddAccount.Parameters.AddWithValue("@Permissions", account.Permissions);
                if (isUserAlreadyExist(account.Name)) { Result = false; }
                else { 
                connection.Open();
                var result = cmdAddAccount.ExecuteNonQuery();
                var nextId = cmdNextAccountId.ExecuteScalar();
                account.Id = (int)(decimal)nextId;
                Accounts.Add(account);
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
                }
            }
            return Result;
        }

        public bool Delete(int Id)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdDeleteAccount = connection.CreateCommand();
                cmdDeleteAccount.CommandText = $"DELETE FROM dbo.Accounts WHERE Id = @Id";
                cmdDeleteAccount.Parameters.AddWithValue("@Id", Id);
                connection.Open();
                var result = cmdDeleteAccount.ExecuteNonQuery();
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
            }
            return Result;
        }

        public bool Update(Account account)
        {
            Accounts = new HashSet<Account> { };
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdUpdateAccount = connection.CreateCommand();

                if (string.IsNullOrEmpty(account.Password)) {

                    cmdUpdateAccount.CommandText = $"UPDATE dbo.Accounts SET Name = @Name WHERE Id = @Id";
                    cmdUpdateAccount.Parameters.AddWithValue("@Id", account.Id);
                    cmdUpdateAccount.Parameters.AddWithValue("@Name", account.Name);
                }

                else
                {
                    cmdUpdateAccount.CommandText = $"UPDATE dbo.Accounts SET Name = @Name, Password = @Password WHERE Id = @Id";
                    cmdUpdateAccount.Parameters.AddWithValue("@Id", account.Id);
                    cmdUpdateAccount.Parameters.AddWithValue("@Name", account.Name);
                    cmdUpdateAccount.Parameters.AddWithValue("@Password", Crypt(account.Password));
                }

                connection.Open();
                account.Permissions = "user";
                Accounts.Add(account);
                var result = cmdUpdateAccount.ExecuteNonQuery();
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
            }
            return Result;
        }

        public bool CanLogin(string Name, string password)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdCanLogin = connection.CreateCommand();
                var passwCrypted = Crypt(password);
                cmdCanLogin.CommandText = $"SELECT Id FROM dbo.Accounts WHERE Name = @Name AND Password = @Password";
                cmdCanLogin.Parameters.AddWithValue("@Name", Name);
                cmdCanLogin.Parameters.AddWithValue("@Password", passwCrypted);
                connection.Open();
                var result = cmdCanLogin.ExecuteScalar();
                if (result != null)
                {
                    Result = true;
                }
                else { Result = false; }
            }
            return Result;
        }

        public bool isUserAlreadyExist(string userName)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAccountAlreadyExist = connection.CreateCommand();
                cmdAccountAlreadyExist.CommandText = $"SELECT Id FROM dbo.Accounts WHERE Name = @Name";
                cmdAccountAlreadyExist.Parameters.AddWithValue("@Name", userName);
                connection.Open();
                var result = cmdAccountAlreadyExist.ExecuteScalar();
                if (result != null)
                {
                    Result = true;
                }
                else { Result = false; }
            }
            return Result;
        }

        public string getIdByUserName(string userName)
        {
            string Result = "0";
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdGetIdByUserName = connection.CreateCommand();
                cmdGetIdByUserName.CommandText = $"SELECT Id FROM dbo.Accounts WHERE Name = @Name";
                cmdGetIdByUserName.Parameters.AddWithValue("@Name", userName);
                connection.Open();
                var Id = cmdGetIdByUserName.ExecuteScalar();
                if (Id == DBNull.Value)
                {
                    Result = "0";
                }
                else { Result = Id.ToString(); }
            }
            return Result;
        }

        public IEnumerable<Account> GetAll()
        {
            Accounts = new HashSet<Account> { };
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAccounts = connection.CreateCommand();
                cmdAccounts.CommandText = "SELECT Id, Name, IsAdmin FROM dbo.Accounts ORDER BY Id";

                connection.Open();
                using (var reader = cmdAccounts.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Account tmpAccount = new Account
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            Permissions = (string)reader["IsAdmin"]
                        };
                        Accounts.Add(tmpAccount);
                    }
                }
            }
            return Accounts.ToList();
        }

        public bool DeprivePermissions(int id)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdDeprivePermissions = connection.CreateCommand();
                cmdDeprivePermissions.CommandText = $"UPDATE dbo.Accounts SET IsAdmin = @Permissions WHERE Id = @Id";
                cmdDeprivePermissions.Parameters.AddWithValue("@Id", id);
                cmdDeprivePermissions.Parameters.AddWithValue("@Permissions", "user");
                connection.Open();
                var result = cmdDeprivePermissions.ExecuteNonQuery();
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
            }
            return Result;
        }

        public bool GivePermissions(int id)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdGivePermissions = connection.CreateCommand();
                    cmdGivePermissions.CommandText = $"UPDATE dbo.Accounts SET IsAdmin = @Permissions WHERE Id = @Id";
                    cmdGivePermissions.Parameters.AddWithValue("@Id", id);
                    cmdGivePermissions.Parameters.AddWithValue("@Permissions", "admin");
                connection.Open();
                var result = cmdGivePermissions.ExecuteNonQuery();
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
            }
            return Result;
        }
    }
}
