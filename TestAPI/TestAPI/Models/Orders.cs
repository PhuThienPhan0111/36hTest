using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestAPI.Models
{
    public class Orders
    {
        public int order_code { get; set; }
        public int customer_code { get; set; }
        public int product_code { get; set; }
        public int shop_code { get; set; }
        public decimal? amount { get; set; }
        public decimal? price { get; set; }
        public DateTime create_time { get; set; }
    }
}