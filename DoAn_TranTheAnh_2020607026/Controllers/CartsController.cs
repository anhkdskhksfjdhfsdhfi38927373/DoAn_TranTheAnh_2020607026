using DoAn_TranTheAnh_2020607026.Models;
using DoAn_TranTheAnh_2020607026.Models.VN_Pay;
using System.Configuration;
using System;
using System.Linq;
using System.Web.Mvc;
using WebGrease;
using System.Web;




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
            if (Session["UserID"] == null)
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
                getcart().AddCart(pro, size);
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
                item = carts.total_Quantity();
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
        public ActionResult CheckOut()
        {
            ListCart carts = Session["Cart"] as ListCart;

            return View(carts);
        }
        public ActionResult SuccessOrder()
        {
            return View();
        }
        public ActionResult VnPay_Return()
        {
            if (Request.QueryString.Count > 0)
            {
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Chuoi bi mat
                var vnpayData = Request.QueryString;
                VnPayLibrary vnpay = new VnPayLibrary();

                foreach (string s in vnpayData)
                {
                    //get all querystring data
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        vnpay.AddResponseData(s, vnpayData[s]);
                    }
                }
                string orderCode = Convert.ToString(vnpay.GetResponseData("vnp_TxnRef"));
                long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                String vnp_SecureHash = Request.QueryString["vnp_SecureHash"];
                String TerminalID = Request.QueryString["vnp_TmnCode"];
                long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                String bankCode = Request.QueryString["vnp_BankCode"];

                bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                    {
                        var itemOrder = db.Orders.FirstOrDefault(x => x.OrderCode == orderCode);
                        if (itemOrder != null)
                        {
                            itemOrder.OrderStatus = "Đã thanh toán";
                            db.Orders.Attach(itemOrder);
                            db.Entry(itemOrder).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        //Thanh toan thanh cong
                        ViewBag.InnerText = "Giao dịch được thực hiện thành công. Cảm ơn quý khách đã sử dụng dịch vụ";
                        //log.InfoFormat("Thanh toan thanh cong, OrderId={0}, VNPAY TranId={1}", orderId, vnpayTranId);
                    }
                    else
                    {

                        ViewBag.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;

                    }

                    ViewBag.ThanhToanThanhCong = "Số tiền thanh toán (VND):" + vnp_Amount.ToString();

                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(FormCollection form)
        {
            int TypePayMent = int.Parse(form["TypePayment"]);
            ListCart carts = Session["Cart"] as ListCart;


            Order order = new Order();
            order.Address_Delivery = form["Address_Delivery"];
            order.PhoneCustomer = form["phone"];
            order.CustomerName = form["Name"];
            order.OrderDate = DateTime.Now;
            order.OrderTotalPrice = carts.Total_Money();
            order.TypePayment = TypePayMent;
            Random rd = new Random();
            order.OrderCode = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
            order.OrderStatus = "Chưa thanh toán";
            db.Orders.Add(order);
            foreach (var item in carts.Listcart)
            {
                OrderDetail orderdetail = new OrderDetail();
                orderdetail.ProductID = item.Product.ProductID;
                orderdetail.OrderQuantity = item.QuantityProductSale;
                orderdetail.TotalPrice = carts.Total_Money();
                orderdetail.OrderID = order.OrderID;
                db.OrderDetails.Add(orderdetail);
            }
            db.SaveChanges();
            carts.Clear_Carts();
            string url = UrlPayment(order.OrderCode);
            if (TypePayMent > 0)
            {
                return Redirect(url);
            }
            else
            {
                return RedirectToAction("SuccessOrder", "Carts");
            }


        }



        public string UrlPayment(string ordercode)
        {
            //Get Config Info
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

            //Get payment input
            //ListCart carts = Session["Cart"] as ListCart;
            //double amount = carts.Total_Money();
            //var price = (long)amount * 100;

            //Save order to db
            Order item = db.Orders.FirstOrDefault(s => s.OrderCode == ordercode);
            var price = (long)item.OrderTotalPrice * 100;
            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (price).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            //if (TypePaymentVN ==1)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "VNPAYQR");
            //}
            //else if (TypePaymentVN == 2)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            //}
            //else if (TypePaymentVN == 3)
            //{
            //    vnpay.AddRequestData("vnp_BankCode", "INTCARD");
            //}

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang Fashion");
            vnpay.AddRequestData("vnp_OrderType", "VNPAY"); //default value: other
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", ordercode.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            //Billing

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

            return paymentUrl;
        }


    }

}