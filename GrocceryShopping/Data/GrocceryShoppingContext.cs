using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GrocceryShopping.Data
{
    public class GrocceryShoppingContext : DbContext
    {
        public GrocceryShoppingContext() : base("name=GrocceryShoppingContext")
        {
        }

        public System.Data.Entity.DbSet<GrocceryShopping.Models.Category> Categories { get; set; }
        public System.Data.Entity.DbSet<GrocceryShopping.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<GrocceryShopping.Models.Order> Order { get; set; }
    }
}
