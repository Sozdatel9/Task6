using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task6.Entities;
using Task6.DalContracts;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Task6.Logic
{
    public class ImageViewer
    {
        public ImageViewer()
        {
        }

        public ImageFile View(int id, string mode=null)
        {
            using (var connection = new SqlConnection(DalProvider.ConnectionString))
            {
                var cmdGetImg = connection.CreateCommand();
                var cmdGetFileName = connection.CreateCommand();
                var cmdGetContentType = connection.CreateCommand();
                connection.Open();
                cmdGetImg.CommandText = $"SELECT Image FROM dbo.Images WHERE Id=@Id";
                cmdGetContentType.CommandText = $"SELECT ContentType FROM dbo.Images WHERE Id=@Id";
                cmdGetFileName.CommandText = $"SELECT FileName FROM dbo.Images WHERE Id=@Id";
                cmdGetImg.Parameters.AddWithValue("@Id", id);
                cmdGetFileName.Parameters.AddWithValue("@Id", id);
                cmdGetContentType.Parameters.AddWithValue("@Id", id);
                byte[] resultImg = (byte[])cmdGetImg.ExecuteScalar();
                var resultFileName = cmdGetFileName.ExecuteScalar();      
                var resultContentType = cmdGetContentType.ExecuteScalar();
                if (resultFileName != null) { 
                ImageFile imageFile = new ImageFile
                {
                    FileName = resultFileName.ToString(),
                    Content = resultImg,
                    ContentType = resultContentType.ToString()
                };
                    return imageFile;

                }
                else
                {
                    string FileN = ""; string ContType = "";
                    if (mode == "award") { FileN = "noAward.jpg"; ContType = "image/jpeg"; }
                    else { FileN = "noAvatar.png"; ContType = "image/png"; }
                    FileStream fStream = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", FileN), FileMode.Open, FileAccess.Read);
                    Byte[] imageBytes = new byte[fStream.Length];
                    fStream.Read(imageBytes, 0, imageBytes.Length);
                    ImageFile defaultImg = new ImageFile
                    {
                        FileName = "default.png",
                        Content = imageBytes,
                        ContentType = ContType
                    };
                    return defaultImg;
                }
            }
        }
    }
}
