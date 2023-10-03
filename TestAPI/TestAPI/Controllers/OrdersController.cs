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
            string query = @"Select * from [Order]";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database);
            }
            List<Orders> orders = new List<Orders>();
            try
            {
                foreach (var _db in _database.Select())
                {
                    orders.Add(new Orders()
                    {
                        order_code = (int)_db.ItemArray[0],
                        customer_code = (int)_db.ItemArray[1],
                        product_code = (int)_db.ItemArray[2],
                        shop_code = (int)_db.ItemArray[3],
                        amount = (decimal)_db.ItemArray[4],
                        price = (decimal)_db.ItemArray[5],
                        create_time = (DateTime)_db.ItemArray[6],
                    });
                }
            }
            catch (Exception ex)
            {

            }
            var items = (from cus in orders
                         select new
                         {
                             orderCode = cus.customer_code,
                             customerName = cus.customer_code,
                             customerEmail = cus.customer_code,
                             shopName = cus.customer_code,
                             shopLocation = cus.customer_code,
                             productName = cus.customer_code,
                             productPrice = cus.customer_code,
                             //name = String.Format("{0} {1}", cus.last_name, cus.first_name),
                             //birthDate = cus.birth_date.ToString("dd/MM/yyyy"),
                             //email = cus.email,
                         }).OrderBy(item => item.orderCode).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
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