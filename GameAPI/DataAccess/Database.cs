using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cassandra;


namespace DataAccess
{
    public class Database : IRepository
    {
        public String CreateUser(String Fname, String Lname, String Uname)
        {
            //using (SqlConnection conn = new SqlConnection())
            //{
            //    var Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            //    conn.ConnectionString = "Data Source=TAVDESK091;Initial Catalog=Game; User Id=sa;Password=test123!@#";
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("insert into UserDB(FirstName, LastName, UserName, AccessToken) values(@fname, @lname, @uname, @accesstoken)", conn);
            //    cmd.Parameters.AddWithValue("@fname", Fname);
            //    cmd.Parameters.AddWithValue("@lname", Lname);
            //    cmd.Parameters.AddWithValue("@uname", Uname);
            //    cmd.Parameters.AddWithValue("@accesstoken", Token);
            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //        conn.Close();
            //        return "Registration Successfully! Token= "+Token;
            //    }
            //    catch (Exception)
            //    {
            //        return "Registeration Failed";
            //    }
            //}

            try
            {
               var Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                var cluster = Cluster.Builder()
                .AddContactPoints("127.0.0.1")
                .Build();
                var session = cluster.Connect("game");
                var insertquery = session.Prepare("INSERT INTO userDB (username, accesstoken,firstname,lastname ) VALUES (?, ?, ?,?)");
                var batch = new BatchStatement()
                  .Add(insertquery.Bind(Uname, Token, Fname, Lname));
                session.Execute(batch);
                return "Registration Successfull! Token=" + Token;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return "Registeration Failed";
            }
        }
        public Boolean CheckTokenValidation(String apikey)
        {
            Boolean validation = false;
            //using (SqlConnection conn = new SqlConnection())
            //{
            //    conn.ConnectionString = "Data Source=TAVDESK091;Initial Catalog=Game; User Id=sa;Password=test123!@#";
            //    string oString = "Select * from UserDB where AccessToken=@token";
            //    SqlCommand cmd = new SqlCommand(oString, conn);
            //    cmd.Parameters.AddWithValue("@token", apikey);
            //    conn.Open();
            //    using (SqlDataReader objectreader = cmd.ExecuteReader())
            //    {
            //        while (objectreader.Read())
            //        {
            //            if (objectreader["AccessToken"].ToString() == apikey)
            //            {
            //                validation = true;
            //                break;
            //            }
            //        }
            //        conn.Close();
            //    }
            //}
                var Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                var cluster = Cluster.Builder()
                .AddContactPoints("127.0.0.1")
                .Build();
                var session = cluster.Connect("game");
                var ps = session.Prepare("Select * from UserDB where accesstoken=? Allow Filtering");
                var statement = ps.Bind(apikey);
                var result=session.Execute(statement);
                if (result != null)
                    validation = true;
                return validation;
        }
    }
}
