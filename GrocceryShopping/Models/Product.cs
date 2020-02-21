using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//Install  entity framework 6 on Tools > Manage Nuget Packages > Microsoft Entity Framework (ver 6.4)
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrocceryShopping.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductTitle { get; set; }
        public string ProductImage { get; set; }
        public string ProductDescription { get; set; }
        public string ProductMeasureUnit { get; set; }
        public string AvailableQuantity { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
    }
}