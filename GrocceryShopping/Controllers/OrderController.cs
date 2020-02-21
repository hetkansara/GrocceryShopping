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
    public class OrderController : Controller
    {
        private GrocceryShoppingContext db = new GrocceryShoppingContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int id)
        {
            List<Order> order = db.Order.SqlQuery("select * from orders where ProductID = @id", new SqlParameter("@id", id)).ToList();

            if (order.Count > 0)
            {
                string orderQuery = "update orders SET Quantity=@Quantity where ProductID = @ProductID";
                SqlParameter[] orderSqlParams = new SqlParameter[2];
                orderSqlParams[0] = new SqlParameter("@ProductID", id);
                orderSqlParams[1] = new SqlParameter("@Quantity", order[0].Quantity + 1);
                db.Database.ExecuteSqlCommand(orderQuery, orderSqlParams);
            }
            else
            {
                string orderQuery = "insert into orders (ProductID, Quantity) values (@ProductID, 1)";
                SqlParameter[] orderSqlParams = new SqlParameter[1];
                orderSqlParams[0] = new SqlParameter("@ProductID", id);
                db.Database.ExecuteSqlCommand(orderQuery, orderSqlParams);
            }
            Product product = db.Products.SqlQuery("select * from products where ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            string query = "update products set AvailableQuantity = @AvailableQuantity where ProductID = @ProductID";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@ProductID", id);
            sqlparams[1] = new SqlParameter("@AvailableQuantity", Int32.Parse(product.AvailableQuantity) - 1);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("Products", "Home", new { id = product.CategoryID });
        }

        public ActionResult List()
        {
            List<Order> orders = db.Order.SqlQuery("select * from orders").ToList();

            List<Product> products = db.Products.SqlQuery("select * from products").ToList();

            OrderView orderView = new OrderView();
            orderView.Orders = orders;
            orderView.Products = products;
            return View(orderView);
        }
        public ActionResult Delete()
        {
            return View();
        }

        public ActionResult DeleteProduct(int id)
        {
            Order order = db.Order.SqlQuery("select * from orders where OrderID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            Product product = db.Products.SqlQuery("select * from products where ProductID = @id", new SqlParameter("@id", order.ProductID)).FirstOrDefault();

            string query = "update products set AvailableQuantity = @AvailableQuantity where ProductID = @ProductID";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@ProductID", order.ProductID);
            sqlparams[1] = new SqlParameter("@AvailableQuantity", Int32.Parse(product.AvailableQuantity) + order.Quantity);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            query = "delete from orders where OrderID = @id";
            sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult Checkout()
        {
            string query = "delete from orders";
            db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("List");
        }
    }
}