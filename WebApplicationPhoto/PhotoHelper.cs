using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Data.SqlClient;
using System.Data;

using System.IO;


namespace WebApplicationPhoto
{
    public class PhotoHelper
    {
        private static string strConn;

        static PhotoHelper()
        {
            strConn = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
        }

        public static int Insert(Photo p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cnn;
            cmd.CommandText = "insert into photos(title,description,photo) values (@title, @description, @photo)";

            SqlParameter title = new SqlParameter("@title", p.Title);
            SqlParameter description = new SqlParameter("@description", p.Description);
            SqlParameter photo = new SqlParameter("@photo", SqlDbType.VarBinary);
            photo.Value = p.PhotoData;
            cmd.Parameters.Add(title);
            cmd.Parameters.Add(description);
            cmd.Parameters.Add(photo);

            cnn.Open();
            int i = cmd.ExecuteNonQuery();
            cnn.Close();

            return i;
        }

        public static int Update(Photo p)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cnn;
            cmd.CommandText = "update photos set title=@title,description=@description,photo.write(@photo, @offset, @length) where photoid = @photoid";

            SqlParameter title = new SqlParameter("@title", p.Title);
            SqlParameter description = new SqlParameter("@description", p.Description);
            SqlParameter photo = new SqlParameter("@photo",SqlDbType.VarBinary);
            photo.Value = p.PhotoData;
            SqlParameter offset = new SqlParameter("@offset", SqlDbType.BigInt);
            offset.Value = 0;
            SqlParameter length = new SqlParameter("@length",p.PhotoData.Length);
            SqlParameter photoid = new SqlParameter("@photoid",p.PhotoID);

            cmd.Parameters.Add(title);
            cmd.Parameters.Add(description);
            cmd.Parameters.Add(photo);
            cmd.Parameters.Add(offset);
            cmd.Parameters.Add(length);
            cmd.Parameters.Add(photoid);

            cnn.Open();
            int i = cmd.ExecuteNonQuery();
            cnn.Close();

            return i;
        }

        public static int Delete(int p)
        {

            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cnn;
            cmd.CommandText = "delete from photos where photoid = @photoid";

            SqlParameter photoid = new SqlParameter("@photoid", p);
            cmd.Parameters.Add(photoid);

            cnn.Open();
            int i = cmd.ExecuteNonQuery();
            cnn.Close();

            return i;

        }

        public static List<Photo> GetAll()
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();
            List<Photo> photos = new List<Photo>();
            byte[] data = new byte[1000];

            cmd.CommandText = "select * from photos order by photoid";
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Photo p = new Photo();
                p.PhotoID = reader.GetInt32(reader.GetOrdinal("PhotoID"));
                p.Title = reader.GetString(reader.GetOrdinal("Title"));
                p.Description = reader.GetString(reader.GetOrdinal("Description"));
                p.PhotoData = (byte[])reader.GetValue(reader.GetOrdinal("Photo"));
                photos.Add(p);
            }
            cnn.Close();
            return photos;
        }

        public static Photo GetByID(int photoid)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();
            byte[] data = new byte[1000];

            cmd.CommandText = "select * from photos where PhotoID = @photoid";
            cmd.Connection = cnn;

            SqlParameter pId = new SqlParameter("@photoid", photoid);
            cmd.Parameters.Add(pId);

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Photo p = new Photo();
            while (reader.Read())
            {
                p.PhotoID = reader.GetInt32(reader.GetOrdinal("PhotoID"));
                p.Title = reader.GetString(reader.GetOrdinal("Title"));
                p.Description = reader.GetString(reader.GetOrdinal("Description"));
                MemoryStream ms = new MemoryStream();
                int index = 0;
                while (true)
                {
                    long count = reader.GetBytes(reader.GetOrdinal("Photo"), index, data, 0, data.Length);
                    if (count == 0)
                    {
                        break;
                    }
                    else
                    {
                        index = index + (int)count;
                        ms.Write(data, 0, (int)count);
                    }
                }
                p.PhotoData = ms.ToArray();
                p.PhotoData = (byte[])reader.GetValue(reader.GetOrdinal("Photo"));
            }
            cnn.Close();
            return p;
        }
    }


}