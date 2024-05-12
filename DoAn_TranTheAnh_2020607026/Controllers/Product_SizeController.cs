using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_TranTheAnh_2020607026.Models;
using PagedList;

namespace DoAn_TranTheAnh_2020607026.Controllers
{
    public class Product_SizeController : Controller
    {
        private Fashion db = new Fashion();

        // GET: Product_Size
        public ActionResult Index(int? page, int? PageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if (PageSize == null)
            {

                PageSize = 8;
            }
            var product_sizes = db.Product_Size.ToList();
            return View(product_sizes.ToPagedList((int)page, (int)PageSize));
        }

        // GET: Product_Size/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Size product_Size = db.Product_Size.Find(id);
            if (product_Size == null)
            {
                return HttpNotFound();
            }
            return View(product_Size);
        }

        // GET: Product_Size/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "NameSize");
            return View();
        }

        // POST: Product_Size/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Product_SizeID,ProductID,SizeID,Quantity")] Product_Size product_Size)
        {
            if (ModelState.IsValid)
            {
                db.Product_Size.Add(product_Size);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", product_Size.ProductID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "NameSize", product_Size.SizeID);
            return View(product_Size);
        }

        // GET: Product_Size/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Size product_Size = db.Product_Size.Find(id);
            if (product_Size == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", product_Size.ProductID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "NameSize", product_Size.SizeID);
            return View(product_Size);
        }

        // POST: Product_Size/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Product_SizeID,ProductID,SizeID,Quantity")] Product_Size product_Size)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_Size).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", product_Size.ProductID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "NameSize", product_Size.SizeID);
            return View(product_Size);
        }

        // GET: Product_Size/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product_Size product_Size = db.Product_Size.Find(id);
            if (product_Size == null)
            {
                return HttpNotFound();
            }
            return View(product_Size);
        }

        // POST: Product_Size/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Size product_Size = db.Product_Size.Find(id);
            db.Product_Size.Remove(product_Size);
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
