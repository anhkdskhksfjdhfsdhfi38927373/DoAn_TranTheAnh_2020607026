using DoAn_TranTheAnh_2020607026.Models;
using System.Linq;
using System.Web.Mvc;




namespace DoAn_TranTheAnh_2020607026.Controllers
{
    public class CartsController : Controller
    {
        // GET: Carts
        Fashion db = new Fashion();
        // GET: Cart
      
        public ListCart getcart()
        {
            ListCart carts = Session["Cart"] as ListCart;
            if (carts == null || Session["Cart"] == null)
            {
                carts = new ListCart();
                Session["Cart"] = carts;
            }
            return carts;
        }
        public ActionResult Index()
        {
            if (Session["UserID"]==null)
            {
                return RedirectToAction("Login", "Page");

            }
            else
            {
                if (Session["Cart"] == null)
                {
                    return RedirectToAction("Index", "Cart");
                }
                ListCart carts = Session["Cart"] as ListCart;
                return View(carts);
            }
            
        }
        [HttpPost]
        public ActionResult AddtoCart(FormCollection form)
        {
            
            int id = int.Parse(form["ProductID"]);
            int sizeId = int.Parse(form["Size"]);
            var size = db.Sizes.SingleOrDefault(s => s.SizeID == sizeId);
            var pro = db.Products.SingleOrDefault(s => s.ProductID == id);
            if (pro != null)
            {
                getcart().AddCart(pro,size);
                
            }
            return RedirectToAction("Index", "Carts");
        }
        public ActionResult UpdateCartup(int id)
        {
            ListCart carts = Session["Cart"] as ListCart;
            carts.UpdatetoCartup(id);
            return RedirectToAction("Index", "Carts");
        }
        public ActionResult UpdateCartdown(int id)
        {
            ListCart carts = Session["Cart"] as ListCart;
            carts.UpdatetoCartdown(id);
            return RedirectToAction("Index", "Carts");
        }
        public PartialViewResult TotalQuantity()
        {
            int item = 0;
            ListCart carts = Session["Cart"] as ListCart;
            if (carts != null)
            {
                 item= carts.total_Quantity();
            }
            ViewBag.total_quantity = item;
            return PartialView("TotalQuantity");
        } 
        public ActionResult Delete_Products(int id)
        {
            ListCart carts = Session["Cart"] as ListCart;
            carts.Delete_Product(id);
            return RedirectToAction("Index", "Carts");
        }
    }
}