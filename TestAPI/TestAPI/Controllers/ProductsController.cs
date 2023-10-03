using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    public class ProductsController : ApiController
    {
        // GET: Product
        string configDB = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
        public HttpResponseMessage Get()
        {
            DataTable _database = new DataTable();
            string query = @"Select * from Product";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _database);
        }

        public string Post(Products products)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"INSERT INTO [dbo].[Product]([name],[price],[shop_code]) VALUES(
                N'" + products.name + @"'
                ,'" + products.price + @"'
                ,'" + products.shop_code + @"'
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