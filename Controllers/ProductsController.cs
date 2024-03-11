using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using CrudOperation.Models;
using Microsoft.Ajax.Utilities;
using PagedList;
using PagedList.Mvc;

namespace CrudOperation.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private EFCodeFirstEntities db = new EFCodeFirstEntities();

        // GET: Products
        public ActionResult Index(string searchBy, string searchValue, int? i)
        {
            var productsQuery = db.Products.Where(p => p.IsActive);

            if (!string.IsNullOrEmpty(searchValue))
            {
                if (searchBy.ToLower() == "productname")
                {
                    productsQuery = productsQuery.Where(p => p.ProductName.ToLower().Contains(searchValue.ToLower()));
                }
                else if (searchBy.ToLower() == "price")
                {
                    decimal searchPrice;
                    if (decimal.TryParse(searchValue, out searchPrice))
                    {
                        productsQuery = productsQuery.Where(p => p.Price == searchPrice);
                    }
                    else
                    {
                        TempData["InfoMessage"] = "Invalid price format. Please enter a valid numeric value.";
                    }
                }
                else if (searchBy.ToLower() == "qty")
                {
                    int searchQty;
                    if (int.TryParse(searchValue, out searchQty))
                    {
                        productsQuery = productsQuery.Where(p => p.Qty == searchQty);
                    }
                    else
                    {
                        TempData["InfoMessage"] = "Invalid quantity format. Please enter a valid numeric value.";
                    }
                }
            }

            return View(productsQuery.ToList().ToPagedList(i ?? 1, 4));
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {

            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,Price,Qty")] Products products)
        {
            if (ModelState.IsValid)
            {
                // Get the currently logged-in user's name
                int currentUserID=0;
                int.TryParse(Session["UserLoginID"].ToString(), out currentUserID);

                products.CreatedBy = currentUserID;
                products.CreatedOn = DateTime.Now;
                products.ModifiedBy = currentUserID;
                products.ModifiedOn = DateTime.Now;

                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(products);
        }




        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,Price,Qty")] Products products)
        {
            if (ModelState.IsValid)
            {
                string currentUserName = User.Identity.Name;

                // Find the existing product in the database
                Products existingProduct = db.Products.Find(products.ProductID);

                if (existingProduct != null)
                {

                    int currentUserID = 0;
                    int.TryParse(Session["UserLoginID"].ToString(), out currentUserID);
                    // Update the properties of the existing product
                    existingProduct.ProductName = products.ProductName;
                    existingProduct.Price = products.Price;
                    existingProduct.Qty = products.Qty;
                    existingProduct.ModifiedBy = currentUserID;
                    existingProduct.ModifiedOn = DateTime.Now;

                    // Mark the existing product as modified
                    db.Entry(existingProduct).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(products);
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            products.IsActive = false; // Set IsDeleted to true (soft-deleted)
            db.Entry(products).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}