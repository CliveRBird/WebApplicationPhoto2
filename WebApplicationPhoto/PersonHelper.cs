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
    public class PersonHelper
    {
        private static string strConn;

        static PersonHelper()
        {
            strConn = ConfigurationManager.ConnectionStrings["connstr_encrypted"].ConnectionString;
        }

        public static int Insert(Person p)
        {

            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cnn;
            cmd.CommandText = "insert into person([NationalInsuranceNumber],[FirstName],[LastName],[Photo]) values (@NationalInsuranceNumber, @FirstName, @LastName, @photo)";

            SqlParameter NationalInsuranceNumber = new SqlParameter("@NationalInsuranceNumber", p.NationalInsuranceNumber);
            SqlParameter FirstName = new SqlParameter("@FirstName", p.FirstName);
            SqlParameter LastName = new SqlParameter("@LastName", p.LastName);
            SqlParameter photo = new SqlParameter("@photo", SqlDbType.VarBinary);
            photo.Value = p.Photo;
            cmd.Parameters.Add(NationalInsuranceNumber);
            cmd.Parameters.Add(FirstName);
            cmd.Parameters.Add(LastName);
            cmd.Parameters.Add(photo);

            cnn.Open();
            int i = cmd.ExecuteNonQuery();
            cnn.Close();

            return i;

        }

        public static int Update(Person p)
        {

            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cnn;
            cmd.CommandText = "update person set NationalInsuranceNumber=@NationalInsuranceNumber,FirstName=@FirstName,Lastname=@LastName,photo.write(@photo, @offset, @length) where personid = @personid";

            SqlParameter NationalInsuranceNumber = new SqlParameter("@NationalInsuranceNumber", p.NationalInsuranceNumber);
            SqlParameter FirstName = new SqlParameter("@FirstName", p.FirstName);
            SqlParameter LastName = new SqlParameter("@LastName", p.LastName);
            SqlParameter photo = new SqlParameter("@photo", SqlDbType.VarBinary);
            photo.Value = p.Photo;
            SqlParameter offset = new SqlParameter("@offset", SqlDbType.BigInt);
            offset.Value = 0;
            SqlParameter length = new SqlParameter("@length", p.Photo.Length);
            SqlParameter personid = new SqlParameter("@personid", p.PersonID);

            cmd.Parameters.Add(NationalInsuranceNumber);
            cmd.Parameters.Add(FirstName);
            cmd.Parameters.Add(LastName);
            cmd.Parameters.Add(photo);
            cmd.Parameters.Add(offset);
            cmd.Parameters.Add(length);
            cmd.Parameters.Add(personid);

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
            cmd.CommandText = "delete from person where personid = @personid";

            SqlParameter personid = new SqlParameter("@personid", p);
            cmd.Parameters.Add(personid);

            cnn.Open();
            int i = cmd.ExecuteNonQuery();
            cnn.Close();

            return i;

        }

        public static List<Person> GetAll()
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();
            List<Person> persons = new List<Person>();

            byte[] data = new byte[1000];

            cmd.CommandText = "select * from person order by personid";
            cmd.Connection = cnn;

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Person p = new Person();
                p.PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                p.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                p.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                p.NationalInsuranceNumber = reader.GetString(reader.GetOrdinal("NationalInsuranceNumber"));
                p.Photo = (byte[])reader.GetValue(reader.GetOrdinal("Photo"));
                persons.Add(p);
            }
            cnn.Close();
            return persons;
        }

        public static Person GetByID(int personid)
        {
            SqlConnection cnn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand();
            byte[] data = new byte[1000];

            cmd.CommandText = "select * from person where PersonID = @personid";
            cmd.Connection = cnn;

            SqlParameter pId = new SqlParameter("@personid", personid);
            cmd.Parameters.Add(pId);

            cnn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            Person p = new Person();
            while (reader.Read())
            {
                p.PersonID = reader.GetInt32(reader.GetOrdinal("PersonID"));
                p.FirstName = reader.GetString(reader.GetOrdinal("FirstName"));
                p.LastName = reader.GetString(reader.GetOrdinal("LastName"));
                p.NationalInsuranceNumber = reader.GetString(reader.GetOrdinal("NationalInsuranceNumber"));
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
                p.Photo = ms.ToArray();
                p.Photo = (byte[])reader.GetValue(reader.GetOrdinal("Photo"));
            }
            cnn.Close();

            return p;
        }

    }


}