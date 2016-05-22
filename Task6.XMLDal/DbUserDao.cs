using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Task6.DalContracts;
using Task6.Entities;
using Task6.Logic;

namespace Task6.DBDal
{
    public class DbUserDao : IUserDao
    {
        HashSet<User> Users;   

        public DbUserDao()
        {

        }

        public bool Create(User user, ImageFile img = null)
        {
            Users = new HashSet<User> { };
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAddImg = connection.CreateCommand();
                var cmdNextImgId = connection.CreateCommand();
                var cmdAddUser = connection.CreateCommand();
                var cmdNextUserID = connection.CreateCommand();
                connection.Open();
                string nextImgIdStr = "";
                if (img != null)
                {   
                    cmdAddImg.CommandText = $"INSERT INTO dbo.Images (Image, FileName, ContentType) VALUES (@Image, @FileName, @ContentType)";
                    cmdAddImg.Parameters.AddWithValue("@Image", img.Content);
                    cmdAddImg.Parameters.AddWithValue("@FileName", img.FileName);
                    cmdAddImg.Parameters.AddWithValue("@ContentType", img.ContentType);
                    cmdNextImgId.CommandText = "SELECT @@IDENTITY";
                    var result0 = cmdAddImg.ExecuteScalar();
                    var nextImgId = cmdNextImgId.ExecuteScalar();
                    nextImgIdStr = nextImgId.ToString();
                }
                else { nextImgIdStr = "0"; }
                cmdAddUser.CommandText = $"INSERT INTO dbo.Users (Name, BirthDate, ImageId) VALUES (@Name, @BirthDate, @ImageId)";
                cmdAddUser.Parameters.AddWithValue("@Name", user.Name);
                cmdAddUser.Parameters.AddWithValue("@BirthDate", user.BirthDate);
                cmdAddUser.Parameters.AddWithValue("@ImageId", nextImgIdStr);
                cmdNextUserID.CommandText = "SELECT @@IDENTITY";
                var result = cmdAddUser.ExecuteNonQuery();
                var nextId = cmdNextUserID.ExecuteScalar();
                user.Id = (int)(decimal)nextId;
                Users.Add(user);
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
            }
            return Result;
        }

        public IEnumerable<User> GetAll()
        {
            Users = new HashSet<User> { };
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdUsers = connection.CreateCommand();
                cmdUsers.CommandText = "SELECT u.Id,  u.Name, u.BirthDate, u.ImageId, a.Title as [Award], a.ImageId as [AwardImg] from dbo.Users as u LEFT JOIN RewardedUsers as r ON r.UserID = u.Id LEFT JOIN Awards as a  ON a.Id = r.AwardId ORDER BY u.Id";
                connection.Open();
                using (var reader = cmdUsers.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string awd = "";
                        int awdImg = 0;
                        if (reader.IsDBNull(4)) { awd = (string)"None"; }
                        if (reader.IsDBNull(5)) { awdImg = 0; }
                        else {
                            awd = (string)reader["Award"];
                            awdImg = (int)reader["AwardImg"];
                        }
                        User tmpUser = new User
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            BirthDate = (DateTime)reader["BirthDate"],
                            ImageId = (int)reader["ImageId"],
                            Award = awd,
                            AwardImgId = awdImg                               
                        };
                        Users.Add(tmpUser);
                    }
                }
            }
            return Users.ToList();
        }


        public IEnumerable<User> GetAllWithoutAwards()
        {
            Users = new HashSet<User> { };
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdUsers2 = connection.CreateCommand();
                cmdUsers2.CommandText = "SELECT * from dbo.Users";
                connection.Open();
                using (var reader = cmdUsers2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User tmpUser = new User
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            BirthDate = (DateTime)reader["BirthDate"],
                            ImageId = (int)reader["ImageId"],
                            Award = "None"
                        };
                        Users.Add(tmpUser);
                    }
                }
            }
            return Users.ToList();
        }


        public IEnumerable<User> GetOnlyAwardedUsers()
        {
            Users = new HashSet<User> { };
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdUsers = connection.CreateCommand();
                cmdUsers.CommandText = "SELECT u.Id,  u.Name, u.BirthDate, u.ImageId, a.Title as [Award], a.ImageId as [AwardImg] from dbo.Users as u LEFT JOIN RewardedUsers as r ON r.UserID = u.Id JOIN Awards as a  ON a.Id = r.AwardId  ORDER BY u.Id";
                connection.Open();
                using (var reader = cmdUsers.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string awd = "";
                        int awdImg = 0;
                        if (reader.IsDBNull(4)) { awd = (string)"None"; }
                        if (reader.IsDBNull(5)) { awdImg = 0; }
                        else {
                            awd = (string)reader["Award"];
                            awdImg = (int)reader["AwardImg"];
                        }
                        User tmpUser = new User
                        {
                            Id = (int)reader["Id"],
                            Name = (string)reader["Name"],
                            BirthDate = (DateTime)reader["BirthDate"],
                            ImageId = (int)reader["ImageId"],
                            Award = awd,
                            AwardImgId = awdImg
                        };
                        Users.Add(tmpUser);
                    }
                }
            }
            return Users.ToList();
        }


        private static void CheckFiles(string fileName)
        {
            using (var xmlFile = File.Open(fileName, FileMode.OpenOrCreate))
            {
            }
        }

        public bool Delete(int Id)
        {
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdDeleteUser = connection.CreateCommand();
                var cmdDeleteUserDependences = connection.CreateCommand();
                cmdDeleteUser.CommandText = $"DELETE FROM dbo.Users WHERE Id = @Id";
                cmdDeleteUserDependences.CommandText = $"DELETE FROM dbo.RewardedUsers WHERE UserID = @Id";
                cmdDeleteUser.Parameters.AddWithValue("@Id", Id);
                cmdDeleteUserDependences.Parameters.AddWithValue("@Id", Id);
                connection.Open();
                var result = cmdDeleteUserDependences.ExecuteNonQuery();
                result = cmdDeleteUser.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool GiveAward(int awardId, int userId)
        {
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdGiveAward = connection.CreateCommand();
                cmdGiveAward.CommandText = $"INSERT INTO dbo.RewardedUsers (UserID, AwardID) VALUES (@userId, @awardId)";
                cmdGiveAward.Parameters.AddWithValue("@awardId", awardId);
                cmdGiveAward.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var result = cmdGiveAward.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool DepriveAward(int awardId, int userId)
        {
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdDepriveAward = connection.CreateCommand();
                cmdDepriveAward.CommandText = $"DELETE FROM dbo.RewardedUsers WHERE AwardID = @awardId AND UserID = @userId";
                cmdDepriveAward.Parameters.AddWithValue("@awardId", awardId);
                cmdDepriveAward.Parameters.AddWithValue("@userId", userId);
                connection.Open();
                var result = cmdDepriveAward.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
