﻿using DoAn_TranTheAnh_2020607026.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;

namespace DoAn_TranTheAnh_2020607026.Controllers
{
    public class PageController : Controller
    {
        Fashion db = new Fashion();

        // GET: Page
       
        public ActionResult Index(int? page,int? PageSize)
        {
            if (page == null)
            {
                page = 1;
            }
            if(PageSize == null)
            {

                PageSize = 8;
            }

            var products = db.Products.ToList();
            return View(products.ToPagedList((int)page,(int)PageSize));
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            
            var item = db.Users.Where(s => s.Email == Email && s.Password == Password).FirstOrDefault();
            if (item != null)
            {
                Session["UserID"] = item.UserID;
                Session["Cart"] = null;
                if (item.RoleID== 0)
                {
                    return RedirectToAction("Index", "Page");
                }
                else if (item.RoleID == 1)
                {
                    Session["Admin"] = item.RoleID;
                    return RedirectToAction("Index", "Page");
                }

            }
            return View();

        }
        public ActionResult Logout()
        {
            Session.Remove("Username");
            Session.Remove("Admin");
            return RedirectToAction("Login", "Page");
        }
        public ActionResult ListproductCategory(int id, int? page, int? PageSize)
        {
            List<Product> list = null;
            
            if (page == null)
            {
                page = 1;
            }
            if (PageSize == null)
            {

                PageSize = 8;
            }
            list = db.Products.Where(m => m.CategoryID == id).ToList();
            return View(list.ToPagedList((int)page, (int)PageSize));
        }
        public ActionResult DetailProduct(int id)
        {
            Product product = null;
            product = db.Products.SingleOrDefault(m => m.ProductID == id);
            List<Rate> rates =db.Rates.Where(s => s.Product.ProductID == id).ToList();
            ViewBag.listrate = rates;
            //ViewBag.product = product;
            return View("DetailProduct",product);
        }
        public PartialViewResult slider()
        {
            List<Slide> listslide = db.Slides.ToList();
            return PartialView(listslide);
        }
        public PartialViewResult ListCategory()
        {
            List<Category> listcategory = db.Categories.ToList();
            return PartialView(listcategory);
        }
        public PartialViewResult listcategory_menu()
        {
            List<Category> listcategory = db.Categories.ToList();
            return PartialView(listcategory);
        }
        [HttpPost]
        public ActionResult Search(FormCollection form, int? page, int? PageSize)
        {
           
                if (page == null)
                {
                    page = 1;
                }
                if (PageSize == null)
                {

                    PageSize = 8;
                }

            string str = form["SearchString"];
            var sanphams = db.Products.Where(s => s.ProductName.Contains(str)).ToList();
            return View(sanphams.ToPagedList((int)page, (int)PageSize));
         
            
           
        }
        
        [HttpPost]
        public ActionResult RateProduct(FormCollection form)
        {
            int rateValue = int.Parse(form["rate"]);
            int ProductId = int.Parse(form["ProductID"]);
            string Comment = form["evaluate_content"];
            var item = db.Products.SingleOrDefault(s => s.ProductID == ProductId);
            int userid = (int)Session["UserID"];
            var user = db.Users.SingleOrDefault(s=>s.UserID == userid);
            if (item != null && rateValue != null && Comment != null && user !=null)
            {
                Rate rate = new Rate();
                rate.RateValue = rateValue;
                rate.Product = item;
                rate.DateRate = DateTime.Now;
                rate.User = user;
                db.Rates.Add(rate);
                db.SaveChanges();
            }
            return RedirectToAction("DetailProduct",new {id=ProductId});
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection form)
        {
            User user = new User();
            user.Username = form["firstname"] + " " + form["lastname"];
            user.Email = form["email"];
            user.Password = form["password"];
            user.Gender = form["Gender"];
            user.RoleID = 0;
            if (form["password"] != form["confirmpassword"])
            {
                ViewBag.errorpassword = "Xác nhận password chưa đúng!";
                return RedirectToAction("Register", "Page");
            }
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Login", "Page");
        }
    }
}