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
    public class DbAwardDao : IAwardDao
    {
        HashSet<Award> Awards;

        public DbAwardDao()
        {
        }
        
        public bool isAwardExist(int awardId)
        {
            bool Result = false;

            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdIsAwardExist = connection.CreateCommand();
                cmdIsAwardExist.CommandText = $"SELECT Id FROM dbo.Awards WHERE Id = @Id";
                cmdIsAwardExist.Parameters.AddWithValue("@Id", awardId);
                connection.Open();
                var result = cmdIsAwardExist.ExecuteScalar();
                if (result == DBNull.Value)
                {
                    Result = false;
                }
                Result = true;
            }
            return Result;
        }

        public bool Create(Award Award, ImageFile img=null)
        {
            Awards = new HashSet<Award> { };
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAddImg = connection.CreateCommand();
                var cmdNextImgId = connection.CreateCommand();
                var cmdAddAward = connection.CreateCommand();
                var cmdNextAwardID = connection.CreateCommand();
                connection.Open(); 

                string nextImgIdStr = "";
                if (img != null) {
                    cmdAddImg.CommandText = $"INSERT INTO dbo.Images (Image, FileName, ContentType) VALUES (@Image, @FileName, @ContentType)";
                    cmdAddImg.Parameters.AddWithValue("@Image", img.Content);
                    cmdAddImg.Parameters.AddWithValue("@ContentType", img.ContentType);
                    cmdAddImg.Parameters.AddWithValue("@FileName", img.FileName);
                    cmdNextImgId.CommandText = "SELECT @@IDENTITY";
                    var result0 = cmdAddImg.ExecuteScalar();
                    var nextImgId = cmdNextImgId.ExecuteScalar();
                    nextImgIdStr =  nextImgId.ToString();
                }
                else { nextImgIdStr = "0"; }
                cmdAddAward.CommandText = $"INSERT INTO dbo.Awards (Title, ImageId) VALUES (@Title, @ImageId)";
                cmdAddAward.Parameters.AddWithValue("@Title", Award.Title);
                cmdAddAward.Parameters.AddWithValue("@ImageId", nextImgIdStr);
                cmdNextAwardID.CommandText = "SELECT @@IDENTITY";
                if (IsAwardAlreadyExist(Award.Title)) { Result = false; }
                else { 
                var result = cmdAddAward.ExecuteNonQuery();
                var nextId = cmdNextAwardID.ExecuteScalar();
                Award.Id = (int)(decimal)nextId;
                Awards.Add(Award);
                if (result == 0)
                {
                    Result = false;
                }
                Result = true;
                }
            }
            return Result; 
        }

        public string GetTitle(int awardId)
        {
            string Result = "";
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdGetAwardTitle = connection.CreateCommand();
                cmdGetAwardTitle.CommandText = $"SELECT Title FROM dbo.Awards WHERE Id = @AwardId";
                cmdGetAwardTitle.Parameters.AddWithValue("@AwardId", awardId);
                connection.Open();
                var Title = cmdGetAwardTitle.ExecuteScalar();
                if (Title == DBNull.Value) { Result = "None"; }
                else
                {
                    Result = (string)Title;
                }
            }
            return Result;
        }

        public bool Delete(int Id)
        {
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdDeleteAward = connection.CreateCommand();
                var cmdDeleteAwardDependences = connection.CreateCommand();
                cmdDeleteAward.CommandText = $"DELETE FROM dbo.Awards WHERE Id = @Id";
                cmdDeleteAwardDependences.CommandText = $"DELETE FROM dbo.RewardedUsers WHERE AwardID = @Id";
                cmdDeleteAward.Parameters.AddWithValue("@Id", Id);
                cmdDeleteAwardDependences.Parameters.AddWithValue("@Id", Id);
                connection.Open();
                var result = cmdDeleteAwardDependences.ExecuteNonQuery();
                result = cmdDeleteAward.ExecuteNonQuery();
                if (result == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public IEnumerable<Award> GetAll()
        {
            Awards = new HashSet<Award> { }; 
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAwards = connection.CreateCommand();
                cmdAwards.CommandText = "SELECT Id, Title, ImageId FROM dbo.Awards ORDER BY Id";

                connection.Open();
                using (var reader = cmdAwards.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Award tmpAward = new Award
                        {
                            Id = (int)reader["Id"],
                            Title = (string)reader["Title"],
                            ImageId = (int)reader["ImageId"]
                        };
                        Awards.Add(tmpAward);
                    }
                }
            }
            return Awards.ToList();
        }

        public bool IsAwardAlreadyExist(string Title)
        {
            bool Result = false;
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdAwardAlreadyExist = connection.CreateCommand();
                cmdAwardAlreadyExist.CommandText = $"SELECT Id FROM dbo.Awards WHERE Title = @Title";
                cmdAwardAlreadyExist.Parameters.AddWithValue("@Title", Title);
                connection.Open();
                var result = cmdAwardAlreadyExist.ExecuteScalar();
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
