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
    public class PatientEncryption
    {

        private static string connstr_encrypted;
        public string FirstName;
        public string LastName;
        public string BirthDate;

        static PatientEncryption()
        {
            //connstr_encrypted = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            connstr_encrypted = ConfigurationManager.ConnectionStrings["connstr_encrypted"].ConnectionString;
        }
        public int insert(string FirstName, string LastName, string BirthYear, string BirthMonth, string BirthDay) 
        {
            int i=0;

            using (SqlConnection connection = new SqlConnection(connstr_encrypted))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandText = @"INSERT INTO [dbo].[Patients] ([SSN], [FirstName], [LastName], [BirthDate]) VALUES (@SSN, @FirstName, @LastName, @BirthDate);";
                        connection.Open();
                        SqlParameter paramSSN = cmd.CreateParameter();
                        paramSSN.ParameterName = @"@SSN";
                        paramSSN.DbType = DbType.AnsiStringFixedLength;
                        paramSSN.Direction = ParameterDirection.Input;
                        paramSSN.Value = "795-73-9838";
                        paramSSN.Size = 11;
                        cmd.Parameters.Add(paramSSN);

                        SqlParameter paramFirstName = cmd.CreateParameter();
                        paramFirstName.ParameterName = @"@FirstName";
                        paramFirstName.DbType = DbType.String;
                        paramFirstName.Direction = ParameterDirection.Input;
                        paramFirstName.Value = FirstName;
                        paramFirstName.Size = 50;
                        cmd.Parameters.Add(paramFirstName);

                        SqlParameter paramLastName = cmd.CreateParameter();
                        paramLastName.ParameterName = @"@LastName";
                        paramLastName.DbType = DbType.String;
                        paramLastName.Direction = ParameterDirection.Input;
                        paramLastName.Value = LastName;
                        paramLastName.Size = 50;
                        cmd.Parameters.Add(paramLastName);

                        SqlParameter paramBirthdate = cmd.CreateParameter();
                        paramBirthdate.ParameterName = @"@BirthDate";
                        paramBirthdate.SqlDbType = SqlDbType.Date;
                        paramBirthdate.Direction = ParameterDirection.Input;

                        paramBirthdate.Value = new DateTime(Int32.Parse(BirthYear), Int32.Parse(BirthMonth), Int32.Parse(BirthDay));
                        cmd.Parameters.Add(paramBirthdate);

                        i = cmd.ExecuteNonQuery();

                        connection.Close();
                    }
                }

            return i;
        }


        public int fetch()
        {
            int i = 0;

            using (SqlConnection connection = new SqlConnection(connstr_encrypted))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {

                    cmd.Connection = connection;
                    cmd.CommandText = @"SELECT top 1 [SSN], [FirstName], [LastName], [BirthDate] FROM [dbo].[Patients] WHERE SSN=@SSN";
                    connection.Open();

                    SqlParameter paramSSN = cmd.CreateParameter();
                    paramSSN.ParameterName = @"@SSN";
                    paramSSN.DbType = DbType.AnsiStringFixedLength;
                    paramSSN.Direction = ParameterDirection.Input;
                    paramSSN.Value = "795-73-9838";
                    paramSSN.Size = 11;
                    cmd.Parameters.Add(paramSSN);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                FirstName = reader[1].ToString();
                                LastName = reader[2].ToString();
                                BirthDate = ((DateTime)reader[3]).ToShortDateString();
                            }
                        }

                    }
                    connection.Close();
                }
            }
            return i;
        }

    }

}