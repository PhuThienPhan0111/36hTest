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
using System.Diagnostics;

namespace TestAPI.Controllers
{
    public class OrdersController : ApiController
    {
        // GET: Orders
        string configDB = ConfigurationManager.ConnectionStrings["sqldb"].ConnectionString;
        public HttpResponseMessage Get()
        {
            DataTable _database_orders = new DataTable();
            string query = @"Select * from [Order]";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database_orders);
            }
            List<Orders> orders = new List<Orders>();
            foreach (var _db in _database_orders.Select())
            {
                orders.Add(new Orders()
                {
                    order_code = (int)_db.ItemArray[0],
                    customer_code = (int)_db.ItemArray[1],
                    product_code = (int)_db.ItemArray[2],
                    shop_code = (int)_db.ItemArray[3],
                    amount = null,
                    price = (decimal)_db.ItemArray[5],
                    create_time = (DateTime)_db.ItemArray[6],
                });
            }

            DataTable _database_customer = new DataTable();
            query = @"Select * from Customer";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database_customer);
            }
            List<Customers> customers = new List<Customers>();
            foreach (var _db in _database_customer.Select())
            {
                customers.Add(new Customers()
                {
                    customer_code = (int)_db.ItemArray[0],
                    first_name = _db.ItemArray[1].ToString(),
                    last_name = _db.ItemArray[2].ToString(),
                    birth_date = (DateTime)_db.ItemArray[3],
                    email = _db.ItemArray[4].ToString(),
                });
            }

            DataTable _database_shop = new DataTable();
            query = @"Select * from Shop";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database_shop);
            }
            List<Shop> shop = new List<Shop>();
         
                foreach (var _db in _database_shop.Select())
                {
                    shop.Add(new Shop()
                    {
                        shop_code = (int)_db.ItemArray[0],
                        name = _db.ItemArray[1].ToString(),
                        location = _db.ItemArray[2].ToString(),
                    });
                }
            DataTable _database_products = new DataTable();
            query = @"Select * from Product";
            using (var con = new SqlConnection(configDB))
            using (var cmd = new SqlCommand(query, con))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                da.Fill(_database_products);
            }

            List<Products> products = new List<Products>();
            foreach (var _db in _database_products.Select())
            {
                products.Add(new Products()
                {
                    product_code = (int)_db.ItemArray[0],
                    name = _db.ItemArray[1].ToString(),
                    price = (decimal)_db.ItemArray[2],
                    shop_code = (int)_db.ItemArray[3],
                });
            }

            int limitUser = 30, limitProducts = 30;
            int limitShop = 3;

            var items = new
            {
                data = (from order in orders
                        let cus = customers.FirstOrDefault(c => order.customer_code == c.customer_code)
                        let shp = shop.FirstOrDefault(s => order.shop_code == s.shop_code)
                        let prod = products.FirstOrDefault(p => order.product_code == p.product_code)
                        select new
                        {
                            orderCode = order.order_code,
                            customerName = String.Format("{0} {1}", cus.last_name.Trim(), cus.first_name.Trim()),
                            customerEmail = cus.email,
                            shopName = shp.name,
                            shopLocation = shp.location,
                            productName = prod.name,
                            productPrice = prod.price == null ? "-" : String.Format("{0} VNĐ", prod.price.Value.ToString("#,0.###")),
                            customerCode = cus.customer_code,
                            shopCode = shp.shop_code,
                            productCode = prod.product_code,
                            price = prod.price,
                        }).OrderBy(item => item.customerEmail).ThenByDescending(item => item.shopLocation).ThenByDescending(item => item.productPrice).ToList(),
                checkData = customers.Count >= limitUser && products.Count >= limitProducts && shop.Count >= limitShop,
                checkDataDisplay = String.Format("Customer {0}/{1} | Products {2}/{3} | Shop {4}/{5}", customers.Count, limitUser , products.Count , limitProducts , shop.Count , limitShop),
            };
            //var items = (from order in orders
            //            let cus = customers.FirstOrDefault(c => order.customer_code == c.customer_code)
            //            let shp = shop.FirstOrDefault(s => order.shop_code == s.shop_code)
            //            let prod = products.FirstOrDefault(p => order.product_code == p.product_code)
            //            select new
            //            {
            //                orderCode = order.order_code,
            //                customerName = String.Format("{0} {1}", cus.last_name.Trim(), cus.first_name.Trim()),
            //                customerEmail = cus.email,
            //                shopName = shp.name,
            //                shopLocation = shp.location,
            //                productName = prod.name,
            //                productPrice = prod.price == null ? "-" : String.Format("{0} VNĐ", prod.price.Value.ToString("#,0.###")),
            //            }).OrderBy(item => item.customerEmail).ThenByDescending(item => item.shopLocation).ThenByDescending(item => item.productPrice).ToList();
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
                ,'0'
                ,'" + orders.price + @"'
                ,'" + DateTime.Now + @"'
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


        public string Put(Orders orders)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"UPDATE [dbo].[Order] SET 
                [customer_code] = N'" + orders.customer_code + @"' 
                ,[product_code] = N'" + orders.product_code + @"' 
                ,[shop_code] = N'" + orders.shop_code + @"' 
                ,[create_time] = N'" + DateTime.Now + @"' 
                ,[price] = N'" + orders.price + @"' 
                where order_code = " + orders.order_code + @"";
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
                string query = @"DELETE FROM [dbo].[Order]
                where order_code = " + id + @"";
                using (var con = new SqlConnection(configDB))
                using (var cmd = new SqlCommand(query, con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.Text;
                    da.Fill(_database);
                }
                return "Deleted";
            }
            catch (Exception ex)
            {
                return "Not OK";
            }
        }
    }
}