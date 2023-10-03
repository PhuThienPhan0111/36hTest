using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    public class ShopController : ApiController
    {
        // GET: Shop
        string configDB = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
        public HttpResponseMessage Get()
        {
            DataTable _database = new DataTable();
            string query = @"Select * from Shop";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _database);
        }

        public string Post(Shop shop)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"INSERT INTO [dbo].[Shop]([name],[location])VALUES(
                N'" + shop.name + @"'
                ,N'" + shop.location + @"'
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
    }
}