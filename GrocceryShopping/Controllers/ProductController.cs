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
using System.IO;

namespace GrocceryShopping.Controllers
{
    public class ProductController : Controller
    {
        private GrocceryShoppingContext db = new GrocceryShoppingContext();
        public ActionResult List(string searchKey)
        {
            string query = "select * from products";
            if (searchKey != "" && searchKey != null)
            {
                query += " where ProductTitle like '%" + searchKey + "%'";
            }
            List<Product> products = db.Products.SqlQuery(query).ToList();
            return View(products);
        }

        public ActionResult Add()
        {
            List<Category> categories = db.Categories.SqlQuery("select * from categories").ToList();

            return View(categories);
        }

        public ActionResult Update(int id)
        {
            List<Category> categories = db.Categories.SqlQuery("select * from categories").ToList();
            ProductUpdate productUpdate = new ProductUpdate();
            productUpdate.Categories = categories;
            Product product = db.Products.SqlQuery("select * from products where ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            productUpdate.Product = product;
            return View(productUpdate);
        }

        [HttpPost]
        public ActionResult UpdateProduct(int id, string ProductCategory, string ProductTitle, string ProductDescription, int AvailableQuantity, string ProductMeasureUnit, HttpPostedFileBase ProductImage)
        {
            string picName = "";
            if (ProductImage != null)
            {
               if (ProductImage.ContentLength > 0)
                {
                    var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                    var extension = Path.GetExtension(ProductImage.FileName).Substring(1);

                    if (valtypes.Contains(extension))
                    {
                        try
                        {
                            picName = id + "." + extension;
                            string path = Path.Combine(Server.MapPath("~/Content/Products/"), picName);
                            ProductImage.SaveAs(path);

                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Pet Image was not saved successfully.");
                            Debug.WriteLine("Exception:" + ex);
                        }
                    }
                }
            }

            if(picName == "")
            {
                Product product = db.Products.SqlQuery("select * from products where ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
                picName = product.ProductImage;
            }
            string query = "update products set CategoryID = @ProductCategory, ProductTitle = @ProductTitle, ProductDescription = @ProductDescription, AvailableQuantity = @AvailableQuantity, ProductMeasureUnit = @ProductMeasureUnit, ProductImage = @ProductImage where ProductID = @ProductID";
            SqlParameter[] sqlparams = new SqlParameter[7];
            sqlparams[0] = new SqlParameter("@ProductCategory", ProductCategory);
            sqlparams[1] = new SqlParameter("@ProductTitle", ProductTitle);
            sqlparams[2] = new SqlParameter("@ProductDescription", ProductDescription);
            sqlparams[3] = new SqlParameter("@AvailableQuantity", AvailableQuantity);
            sqlparams[4] = new SqlParameter("@ProductMeasureUnit", ProductMeasureUnit);
            sqlparams[5] = new SqlParameter("@ProductImage", picName);
            sqlparams[6] = new SqlParameter("@ProductID", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult AddProduct(string ProductCategory, string ProductTitle, string ProductDescription, int AvailableQuantity, string ProductMeasureUnit)
        {
            string query = "insert into products (ProductTitle, ProductDescription, AvailableQuantity, ProductMeasureUnit, CategoryID) values (@ProductTitle, @ProductDescription, @AvailableQuantity, @ProductMeasureUnit, @ProductCategory)";
            SqlParameter[] sqlparams = new SqlParameter[5];
            sqlparams[0] = new SqlParameter("@ProductCategory", ProductCategory);
            sqlparams[1] = new SqlParameter("@ProductTitle", ProductTitle);
            sqlparams[2] = new SqlParameter("@ProductDescription", ProductDescription);
            sqlparams[3] = new SqlParameter("@AvailableQuantity", AvailableQuantity);
            sqlparams[4] = new SqlParameter("@ProductMeasureUnit", ProductMeasureUnit);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            Product product = db.Products.SqlQuery("select * from products where ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            return View(product);
        }

        public ActionResult DeleteProduct(int id)
        {
            string query = "delete from products where ProductID = @id";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        public ActionResult View(int id)
        {
            List<Category> categories = db.Categories.SqlQuery("select * from categories").ToList();
            ProductUpdate productUpdate = new ProductUpdate();
            productUpdate.Categories = categories;
            Product product = db.Products.SqlQuery("select * from products where ProductID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            productUpdate.Product = product;
            return View(productUpdate);
        }
    }
}