using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Cassandra;

namespace DataAccess
{
    public class Logger : ILogger
    {
        public void LogMessage(string request, string response,String exceptions, String comment)
        {
            //using (SqlConnection conn = new SqlConnection())
            //{
            //    conn.ConnectionString = "Data Source=TAVDESK091;Initial Catalog=Game; User Id=sa;Password=test123!@#";
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand("insert into LoggingDB(Request,Response,Exception,Comment) values(@REquest,@REsponse,@EXceptions,@COmment)", conn);
            //    cmd.Parameters.AddWithValue("@REquest", request);
            //    cmd.Parameters.AddWithValue("@REsponse", response);
            //    cmd.Parameters.AddWithValue("@EXceptions", exceptions);
            //    cmd.Parameters.AddWithValue("@COmment", comment);

            //    try
            //    {
            //        cmd.ExecuteNonQuery();
            //        conn.Close();

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //}
            try
            {
                var cluster = Cluster.Builder()
                .AddContactPoints("127.0.0.1")
                .Build();
                Random random = new Random();
                int idcounter= random.Next(1000);
                var session = cluster.Connect("game");
                var insertquery = session.Prepare("INSERT INTO LoggingDB (id,request, response,exception,comment) VALUES (?,?, ?, ?,?)");
                var batch = new BatchStatement()
                  .Add(insertquery.Bind(idcounter,request,response, exceptions, comment));
                session.Execute(batch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
