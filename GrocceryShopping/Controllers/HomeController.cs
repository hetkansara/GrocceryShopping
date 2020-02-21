using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrocceryShopping.Models;
using GrocceryShopping.Models.ViewModels;
using GrocceryShopping.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GrocceryShopping.Controllers
{
    public class HomeController : Controller
    {
        private GrocceryShoppingContext db = new GrocceryShoppingContext();

        public ActionResult Index()
        {
            List<Category> categories = db.Categories.SqlQuery("select * from categories").ToList();

            return View(categories);
        }

        public ActionResult Products(int id)
        {
            List<Product> products = db.Products.SqlQuery("select * from products where CategoryID = @id", new SqlParameter("@id", id)).ToList();
            return View(products);
        }

    }
}