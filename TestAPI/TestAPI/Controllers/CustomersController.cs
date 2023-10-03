using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    public class CustomersController : ApiController
    {
        string configDB = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
        // GET: Customers
        public HttpResponseMessage Get()
        {
            DataTable _database = new DataTable();
            string query = @"Select * from Customer";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _database);
        }
        public string Post(Customers customers)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"INSERT INTO [dbo].[Customer]([first_name],[last_name],[birth_date],[email]) VALUES(
                N'" + customers.first_name + @"'
                ,N'" + customers.last_name + @"'
                ,'" + customers.birth_date + @"'
                ,'" + customers.email + @"'
                )";
                using (var con = new SqlConnection(configDB))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(_database);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return "Not OK";
            }
        }

        public string Put(Customers customers)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"UPDATE [dbo].[Customer] SET 
                [first_name] = N'" + customers.first_name + @"' 
                ,[last_name] = N'" + customers.last_name + @"' 
                ,[birth_date] = N'" + customers.birth_date + @"' 
                ,[email] = N'" + customers.email + @"' 
                where customer_code = " + customers.customer_code + @"";
                using (var con = new SqlConnection(configDB))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(_database);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return "Not OK";
            }
        }

        public string Delete(int id)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"DELETE FROM [dbo].[Customer]
                where customer_code = " + id + @"";
                using (var con = new SqlConnection(configDB))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(_database);
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return "Not OK";
            }
        }
    }
}