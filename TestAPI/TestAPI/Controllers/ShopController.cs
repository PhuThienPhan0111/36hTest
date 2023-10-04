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
            List<Shop> shop = new List<Shop>();
            try
            {
                foreach (var _db in _database.Select())
                {
                    shop.Add(new Shop()
                    {
                        shop_code = (int)_db.ItemArray[0],
                        name = _db.ItemArray[1].ToString(),
                        location = _db.ItemArray[2].ToString(),
                    });
                }
            }
            catch (Exception ex)
            {

            }
            var items = (from shp in shop
                         select new
                         {
                             shopCode = shp.shop_code,
                             name = shp.name,
                             location = shp.location,
                         }).OrderByDescending(item => item.location).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, items);
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

        public string Put(Shop shop)
        {
            try
            {
                DataTable _database = new DataTable();
                string query = @"UPDATE [dbo].[Customer] SET 
                [name] = N'" + shop.name + @"' 
                ,[location] = N'" + shop.location + @"' 
                where customer_code = " + shop.shop_code + @"";
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
                string query = @"DELETE FROM [dbo].[Shop]
                where shop_code = " + id + @"";
                query += @"DELETE FROM [dbo].[Order]
                where shop_code = " + id + @";";
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