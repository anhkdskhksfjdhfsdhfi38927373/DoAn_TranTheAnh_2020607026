namespace DoAn_TranTheAnh_2020607026.Models
{
    using Microsoft.AspNetCore.Http.Internal;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Security.AccessControl;

    public partial class Cart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CartID { get; set; }

        public int? UserID { get; set; }

        public int? ProductID { get; set; }
        public int QuantityProductSale { get; set; }


        public virtual Product Product { get; set; }

        public virtual User User { get; set; }
    }
    public class ListCart
    {
        List<Cart> listcart = new List<Cart>();
        public IEnumerable<Cart> Listcart
        {
            get { return listcart; }
        }
        public void AddCart(Product product, int quantity = 1)
        {
            var item = listcart.FirstOrDefault(s => s.Product.ProductID == product.ProductID);
            if (item == null)
            {
                listcart.Add(new Cart
                {
                    Product = product,
                    QuantityProductSale = quantity
                });
            }
            else
            {
                item.QuantityProductSale += quantity;
            }
        }
        public void UpdatetoCartup(int id)
        {
            var item = listcart.Find(s =>s.Product.ProductID == id) ;
            if(item != null)
            {
                item.QuantityProductSale +=1;
            }
        }
        public void UpdatetoCartdown(int id)
        {
            var item = listcart.Find(s => s.Product.ProductID == id);
            if (item != null)
            {
                if(item.QuantityProductSale >= 1) 
                {
                    item.QuantityProductSale -= 1;
                }
                
            }
        }
        public double Total_Money()
        {
            var total = Listcart.Sum(s=>s.Product.newprice(s.Product.SaleOff,s.Product.Price) * s.QuantityProductSale);
            return (double)total;
        }
    }
}
