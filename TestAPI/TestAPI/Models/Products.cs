using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI.Models
{
    public class Products
    {        
        public int product_code { get; set; }
        public string name { get; set; }
        public decimal? price { get; set; }
        public int shop_code { get; set; }
    }
}