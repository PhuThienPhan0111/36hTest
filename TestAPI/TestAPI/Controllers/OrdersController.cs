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
    public class OrdersController : ApiController
    {
        // GET: Orders
        string configDB = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
        public HttpResponseMessage Get()
        {
            DataTable _database = new DataTable();
            string query = @"Select * from Order";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database);
            }
            return Request.CreateResponse(HttpStatusCode.OK, _database);
        }

        public string Post(Orders orders)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"INSERT INTO [dbo].[Order]([customer_code],[product_code],[shop_code],[amount],[price],[create_time]) VALUES(
                '" + orders.customer_code + @"'
                ,'" + orders.product_code + @"'
                ,'" + orders.shop_code + @"'
                ,'" + orders.amount + @"'
                ,'" + orders.price + @"'
                ,'" + orders.create_time + @"'
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