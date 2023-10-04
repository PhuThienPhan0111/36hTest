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

            List<Products> products = new List<Products>();
            try
            {
                foreach (var _db in _database.Select())
                {
                    products.Add(new Products()
                    {
                        product_code = (int)_db.ItemArray[0],
                        name = _db.ItemArray[1].ToString(),
                        price = (decimal)_db.ItemArray[2],
                        shop_code = (int)_db.ItemArray[3],
                    });
                }
            }
            catch (Exception ex)
            {

            }
            var items = (from prod in products
                         select new
                         {
                             productCode = prod.product_code,
                             name = prod.name,
                             priceDisplay = prod.price == null ? "-" : String.Format("{0} VNĐ", prod.price.Value.ToString("#,0.###")),
                             shopCode = prod.shop_code,
                             price = prod.price
                         }).OrderByDescending(item => item.price).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
        }

        public HttpResponseMessage Get(int id)
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

            List<Products> products = new List<Products>();
            try
            {
                foreach (var _db in _database.Select())
                {
                    if((int)_db.ItemArray[3] == id)
                    {
                        products.Add(new Products()
                        {
                            product_code = (int)_db.ItemArray[0],
                            name = _db.ItemArray[1].ToString(),
                            price = (decimal)_db.ItemArray[2],
                            shop_code = (int)_db.ItemArray[3],
                        });
                    }                   
                }
            }
            catch (Exception ex)
            {

            }
            var items = (from prod in products
                         select new
                         {
                             productCode = prod.product_code,
                             name = prod.name,
                             priceDisplay = prod.price == null ? "-" : String.Format("{0} VNĐ", prod.price.Value.ToString("#,0.###")),
                             shopCode = prod.shop_code,
                             price = prod.price
                         }).OrderByDescending(item => item.price).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
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

        public string Put(Products products)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"UPDATE [dbo].[Customer] SET 
                [name] = N'" + products.name + @"' 
                ,[price] = N'" + products.price + @"' 
                ,[shop_code] = N'" + products.shop_code + @"' 
                where customer_code = " + products.product_code + @"";
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
                string query = @"DELETE FROM [dbo].[Product]
                where product_code = " + id + @"";
                query += @"DELETE FROM [dbo].[Order]
                where product_code = " + id + @";";
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