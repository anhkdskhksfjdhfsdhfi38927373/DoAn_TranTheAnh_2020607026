using DoAn_TranTheAnh_2020607026.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace DoAn_TranTheAnh_2020607026.Controllers
{
    public class CartController : Controller
    {
        
        Fashion db = new Fashion();
        // GET: Cart
        public ListCart getcart()
        {
            ListCart carts = Session["Cart"] as ListCart;
            if(carts == null || Session["Cart"] == null)
            {
                carts = new ListCart();
                Session["Cart"] = carts;
            }
            return carts;
        }
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Cart");
            }
            ListCart carts = Session["Cart"] as ListCart;
            
            return View(carts);
        }
        public ActionResult AddtoCart(int id)
        {
            var pro = db.Products.SingleOrDefault(s=>s.ProductID == id);
            if(pro != null)
            {
                getcart().AddCart(pro);
            }
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult UpdateCartup(int id)
        {
            ListCart carts = Session["Cart"] as ListCart;
            carts.UpdatetoCartup(id);
            return RedirectToAction("Index", "Cart");
        }
        public ActionResult UpdateCartdown(int id)
        {
            ListCart carts = Session["Cart"] as ListCart;
            carts.UpdatetoCartdown(id);
            return RedirectToAction("Index", "Cart");
        }
    }
}