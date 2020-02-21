using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrocceryShopping.Models;
using GrocceryShopping.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GrocceryShopping.Controllers
{
    public class CategoryController : Controller
    {
        private GrocceryShoppingContext db = new GrocceryShoppingContext();
        public ActionResult List(string searchKey)
        {
            string query = "select * from categories";
            if(searchKey != "" && searchKey != null)
            {
                query += " where CategoryTitle like '%" + searchKey + "%'";
            }
            List<Category> categories = db.Categories.SqlQuery(query).ToList();

            return View(categories);
        }

        public ActionResult Update(int id)
        {
            Category category = db.Categories.SqlQuery("select * from categories where CategoryID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(category);
        }

        public ActionResult Delete(int id)
        {
            Category category = db.Categories.SqlQuery("select * from categories where CategoryID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(category);
        }

        public ActionResult DeleteCategory(int id)
        {
            string query = "delete from categories where CategoryID = @id";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult UpdateCategory(int id, string CategoryTitle)
        {
            Debug.WriteLine(id);
            string query = "update categories set CategoryTitle = @CategoryTitle where CategoryID = @CategoryID";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@CategoryTitle", CategoryTitle);
            sqlparams[1] = new SqlParameter("@CategoryID", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(string CategoryTitle)
        {
            string query = "insert into categories (CategoryTitle) values (@CategoryTitle)";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@CategoryTitle", CategoryTitle);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
    }
}