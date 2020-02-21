using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrocceryShopping.Models.ViewModels
{
    public class OrderView
    {
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
    }
}