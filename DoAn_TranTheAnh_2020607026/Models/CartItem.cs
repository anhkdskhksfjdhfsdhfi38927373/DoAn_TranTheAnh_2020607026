namespace DoAn_TranTheAnh_2020607026.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class CartItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CartItemID { get; set; }

        public int? QuantityProductSale { get; set; }

        public int ProductID { get; set; }

        public int? SizeID { get; set; }

        public virtual Size Size { get; set; }

        public virtual Product Product { get; set; }
    }
    public class ListCart
    {
        List<CartItem> listcart = new List<CartItem>();
        public IEnumerable<CartItem> Listcart
        {
            get { return listcart; }
        }
        public void AddCart(Product product,Size size, int quantity = 1)
        {
            var item = listcart.FirstOrDefault(s => s.Product.ProductID == product.ProductID);
            var sizeitem = listcart.FirstOrDefault(s => s.Size.SizeID == size.SizeID);
            if (item == null && sizeitem ==null)
            {
                listcart.Add(new CartItem
                {
                    Product = product,
                    Size = size,
                    QuantityProductSale = quantity
                });
            }
            if(item!=null && sizeitem == null)
            {
                listcart.Add(new CartItem
                {
                    Product = product,
                    Size = size,
                    QuantityProductSale = quantity
                });
            }
            
        }
        public void UpdatetoCartup(int id)
        {
            var item = listcart.Find(s => s.Product.ProductID == id);
            if (item != null)
            {
                item.QuantityProductSale += 1;
            }
        }
        public void UpdatetoCartdown(int id)
        {
            var item = listcart.Find(s => s.Product.ProductID == id);
            if (item != null)
            {
                if (item.QuantityProductSale >= 1)
                {
                    item.QuantityProductSale -= 1;
                }

            }
        }
        public double Total_Money()
        {
            var total = Listcart.Sum(s => s.Product.newprice(s.Product.SaleOff, s.Product.Price) * s.QuantityProductSale);
            return (double)total;
        }
        public void Clear_Carts()
        {
            listcart.Clear();
        }
        public int total_Quantity()
        {
            return (int)listcart.Sum(s => s.QuantityProductSale);
        }
        public void Delete_Product(int productid)
        {
            listcart.RemoveAll(s => s.Product.ProductID == productid);
        }
    }
}
